namespace Morpeh.TestSuite.Editor {
    using System.Linq;
    using NUnit.Framework;

    [TestFixture(Category = "Morpeh/Entity")]
    public class EntityComponentMigrationTests {
        private World world;

        private ComponentsCache<TestComponent> testComponentCache;

        private Entity originEntity;
        private Entity migratedEntity;

        [SetUp]
        public void Setup() {
            this.world = World.Create();

            this.testComponentCache = this.world.GetCache<TestComponent>();

            this.originEntity   = this.world.CreateEntity();
            this.migratedEntity = this.world.CreateEntity();

            this.originEntity.AddComponent<TestMarker>(); // just to keep alive
            this.migratedEntity.AddComponent<TestMarker>();
        }

        [TearDown]
        public void TearDown() {
            this.world.Dispose();
            this.world = null;
        }

        [Test]
        public void Entity_MigrateComponent_Override_Set() {
            this.originEntity.SetComponent(new TestComponent {data0 = 1});
            this.migratedEntity.SetComponent(new TestComponent());

            this.testComponentCache.MigrateComponent(this.originEntity, this.migratedEntity, overwrite: true);

            // cache updated
            Assert.IsFalse(this.originEntity.Has<TestComponent>());
            Assert.IsTrue(this.migratedEntity.Has<TestComponent>());

            // archetypes updated
            Assert.IsFalse(FilterContainsEntity<TestComponent>(this.originEntity));
            Assert.IsTrue(FilterContainsEntity<TestComponent>(this.migratedEntity));

            // data migrated
            Assert.AreEqual(1, this.migratedEntity.GetComponent<TestComponent>().data0);
        }

        [Test]
        public void Entity_MigrateComponent_Override_Add() {
            this.originEntity.SetComponent(new TestComponent {data0 = 1});

            this.testComponentCache.MigrateComponent(this.originEntity, this.migratedEntity, overwrite: true);

            Assert.IsFalse(this.originEntity.Has<TestComponent>());
            Assert.IsTrue(this.migratedEntity.Has<TestComponent>());

            Assert.IsFalse(FilterContainsEntity<TestComponent>(this.originEntity));
            Assert.IsTrue(FilterContainsEntity<TestComponent>(this.migratedEntity));

            Assert.AreEqual(1, this.migratedEntity.GetComponent<TestComponent>().data0);
        }

        [Test]
        public void Entity_MigrateComponent_NoOverride_None() {
            this.originEntity.SetComponent(new TestComponent {data0 = 1});
            this.migratedEntity.SetComponent(new TestComponent());

            this.testComponentCache.MigrateComponent(this.originEntity, this.migratedEntity, overwrite: false);

            Assert.IsFalse(this.originEntity.Has<TestComponent>());
            Assert.IsTrue(this.migratedEntity.Has<TestComponent>());

            Assert.IsFalse(FilterContainsEntity<TestComponent>(this.originEntity));
            Assert.IsTrue(FilterContainsEntity<TestComponent>(this.migratedEntity));

            // data NOT migrated
            Assert.AreEqual(0, this.migratedEntity.GetComponent<TestComponent>().data0);
        }

        [Test]
        public void Entity_MigrateComponent_NoOverride_Add() {
            this.originEntity.SetComponent(new TestComponent {data0 = 1});

            this.testComponentCache.MigrateComponent(this.originEntity, this.migratedEntity, overwrite: false);

            Assert.IsFalse(this.originEntity.Has<TestComponent>());
            Assert.IsTrue(this.migratedEntity.Has<TestComponent>());

            Assert.IsFalse(FilterContainsEntity<TestComponent>(this.originEntity));
            Assert.IsTrue(FilterContainsEntity<TestComponent>(this.migratedEntity));

            Assert.AreEqual(1, this.migratedEntity.GetComponent<TestComponent>().data0);
        }

        private static bool FilterContainsEntity<TComponent>(Entity e) where TComponent : struct, IComponent {
            var filter = e.world.Filter.With<TComponent>();
            filter.world.UpdateFilters();
            return filter.Any(entity => entity == e);
        }
    }
}