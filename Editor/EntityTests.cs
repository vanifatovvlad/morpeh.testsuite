namespace Morpeh.TestSuite.Editor {
    using NUnit.Framework;

    [TestFixture(Category = "Morpeh/Entity")]
    public class EntityTests {
        private World world;

        [SetUp]
        public void Setup() => this.world = World.Create();

        [TearDown]
        public void TearDown() {
            this.world.Dispose();
            this.world = null;
        }
        
        //Component
        [Test]
        public void Entity_AddComponent_Return_Default_Component() {
            var entity = this.world.CreateEntity();

            ref var component = ref entity.AddComponent<TestComponent>();
            
            Assert.AreEqual(component, default(TestComponent));
        }
        
        [Test]
        public void Entity_GetComponent_Return_Changed_Component() {
            var entity = this.world.CreateEntity();

            ref var component = ref entity.AddComponent<TestComponent>();
            component.data0 = 123;
            component.data1 = 1.234f;
            component.data2 = true;

            component = ref entity.GetComponent<TestComponent>();
            
            Assert.AreNotEqual(component, default(TestComponent));
        }
        
        [Test]
        public void Entity_Has_Return_False_If_Component_Not_Exists() {
            var entity = this.world.CreateEntity();

            var has = entity.Has<TestComponent>();
            
            Assert.False(has);
        }
        
        [Test]
        public void Entity_Has_Return_True_If_Component_Exists() {
            var entity = this.world.CreateEntity();

            entity.AddComponent<TestComponent>();

            var has = entity.Has<TestComponent>();
            
            Assert.True(has);
        }
        
        [Test]
        public void Entity_SetComponent_Change_Component() {
            var entity = this.world.CreateEntity();

            ref var component = ref entity.AddComponent<TestComponent>();
            entity.SetComponent(new TestComponent{data0 = 123, data1 = 1.234f, data2 = true});

            component = ref entity.GetComponent<TestComponent>();
            
            Assert.AreNotEqual(component, default(TestComponent));
        }
        
        [Test]
        public void Entity_SetComponent_Add_Component() {
            var entity = this.world.CreateEntity();

            entity.SetComponent(new TestComponent{data0 = 123, data1 = 1.234f, data2 = true});

            ref var component = ref entity.GetComponent<TestComponent>();
            
            Assert.AreNotEqual(component, default(TestComponent));
        }
        
        [Test]
        public void Entity_RemoveComponent_Delete_Component_From_Entity() {
            var entity = this.world.CreateEntity();

            entity.AddComponent<TestComponent>();
            entity.AddComponent<TestMarker>();
            entity.RemoveComponent<TestComponent>();

            var has = entity.Has<TestComponent>();
            
            Assert.False(has);
        }

        [Test]
        public void Entity_RemoveComponent_Delete_Last_Component_Force_Delete_Entity_From_World() {
            var entity = this.world.CreateEntity();

            entity.AddComponent<TestComponent>();
            entity.RemoveComponent<TestComponent>();
            this.world.UpdateFilters();

            Assert.True(entity.isDisposed);
        }
        
        //Marker
        [Test]
        public void Entity_Operations_With_Marker_Is_Just_Work() {
            var entity = this.world.CreateEntity();

            entity.AddComponent<TestMarker>();
            entity.SetComponent(new TestMarker());
            entity.GetComponent<TestMarker>();
            entity.GetComponent<TestMarker>(out var exits);
            var has = entity.Has<TestMarker>();
            
            Assert.True(exits);
            Assert.True(has);
        }
    }
}