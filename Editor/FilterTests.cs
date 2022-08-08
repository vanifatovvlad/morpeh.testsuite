namespace Morpeh.TestSuite.Editor {
    using NUnit.Framework;

    [TestFixture(Category = "Morpeh/Filter")]
    public class FilterTests {
        private const float DELTA_TIME = 0.1f;

        private World world;

        private static TestSystemBase[] TestSystems => new TestSystemBase[] {
            new TestUpdateSystem(),
            new TestFixedSystem(),
            new TestLateSystem(),
        };

        [SetUp]
        public void Setup() => this.world = World.Create();

        [TearDown]
        public void TearDown() {
            this.world.Dispose();
            this.world = null;
        }

        [Test]
        public void Filter_RemoveEntity_Remove_Last_Entity_Should_Cause_Empty_Filter() {
            var filter = this.world.Filter.With<TestComponent>();
            var entity = this.world.CreateEntity();

            entity.AddComponent<TestComponent>();
            this.world.UpdateFilters();
            this.world.RemoveEntity(entity);
            this.world.UpdateFilters();

            Assert.IsTrue(filter.IsEmpty());
        }

        [Test]
        public void Filter_UpToDate_On_System_Dispose() {
            var lengthOnDispose = -1;

            var system = new TestUpdateSystem();

            system.DisposeAction = f => lengthOnDispose = f.GetLengthSlow();

            var systemGroup = this.world.CreateSystemsGroup();
            systemGroup.AddSystem(system);
            this.world.AddSystemsGroup(0, systemGroup);

            this.world.Update(DELTA_TIME);

            this.world.CreateEntity().AddComponent<TestComponent>();

            systemGroup.RemoveSystem(system);
            this.world.Update(DELTA_TIME);

            Assert.AreEqual(1, lengthOnDispose);
        }

        [Test]
        public void Filter_UpToData_OnSystem_Dispose_When_Filter_Modified_On_Dispose() {
            var lengthOnDispose = -1;

            var system1 = new TestUpdateSystem();
            var system2 = new TestUpdateSystem();

            system1.DisposeAction = f => f.world.CreateEntity().AddComponent<TestComponent>();
            system2.DisposeAction = f => lengthOnDispose = f.GetLengthSlow();

            var systemGroup = this.world.CreateSystemsGroup();
            systemGroup.AddSystem(system1);
            systemGroup.AddSystem(system2);
            this.world.AddSystemsGroup(0, systemGroup);

            this.world.Update(DELTA_TIME);

            systemGroup.RemoveSystem(system1);
            systemGroup.RemoveSystem(system2);
            this.world.Update(DELTA_TIME);

            Assert.AreEqual(1, lengthOnDispose);
        }

        [Test]
        [TestCaseSource(nameof(TestSystems))]
        public void Filter_UpToDate_On_System_Update(TestSystemBase system) {
            var lengthOnAwake  = -1;
            var lengthOnUpdate = -1;

            system.AwakeAction  = f => lengthOnAwake  = f.GetLengthSlow();
            system.UpdateAction = f => lengthOnUpdate = f.GetLengthSlow();

            this.world.CreateEntity().AddComponent<TestComponent>(); // First component

            var systemGroup = this.world.CreateSystemsGroup();
            systemGroup.AddSystem(system);
            this.world.AddSystemsGroup(0, systemGroup);

            this.world.Update(DELTA_TIME); // Initialize system

            this.world.CreateEntity().AddComponent<TestComponent>(); // Second component

            system.DoWorldUpdate(this.world, DELTA_TIME); // Actual update

            Assert.AreEqual(1, lengthOnAwake);
            Assert.AreEqual(2, lengthOnUpdate);
        }
    }
}