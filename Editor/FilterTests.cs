namespace Morpeh.TestSuite.Editor {
    using NUnit.Framework;

    [TestFixture(Category = "Morpeh/Filter")]
    public class FilterTests {
        private World world;

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
    }
}