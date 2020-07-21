namespace Morpeh.TestSuite.Editor {
    using NUnit.Framework;

    [TestFixture(Category = "Morpeh/World")]
    public class WorldTests {
        private World world;

        [SetUp]
        public void Setup() => this.world = World.Create();

        [TearDown]
        public void TearDown() {
            this.world.Dispose();
            this.world = null;
        }

        [Test]
        public void World_Entities_Capacity_Is_Same_As_Const_Capacity_By_Default() {
            Assert.AreEqual(this.world.entitiesCapacity, Constants.DEFAULT_WORLD_ENTITIES_CAPACITY);
        }

        [Test]
        public void World_Entities_Length_Is_Same_As_Capacity_By_Default() {
            Assert.AreEqual(this.world.entitiesCapacity, this.world.entities.Length);
        }

        [Test]
        public void World_Entities_Public_Length_Is_Zero_By_Default() {
            Assert.AreEqual(this.world.entitiesLength, 0);
        }

        [Test]
        [TestCase(1)]
        [TestCase(12)]
        [TestCase(123)]
        [TestCase(1234)]
        public void World_Entities_Public_Length_Is_Grow_Right(int countEntities) {
            for (int i = 0, length = countEntities; i < length; i++) {
                this.world.CreateEntity();
            }

            Assert.AreEqual(this.world.entitiesLength, countEntities);
        }

        [Test]
        [TestCase(Constants.DEFAULT_WORLD_ENTITIES_CAPACITY + 1, Constants.DEFAULT_WORLD_ENTITIES_CAPACITY * 2)]
        [TestCase(Constants.DEFAULT_WORLD_ENTITIES_CAPACITY * 2 + 1, Constants.DEFAULT_WORLD_ENTITIES_CAPACITY * 2 * 2)]
        public void World_Entities_Capacity_Is_Grow_Right(int countEntities, int expected) {
            for (int i = 0, length = countEntities; i < length; i++) {
                this.world.CreateEntity();
            }

            Assert.AreEqual(this.world.entities.Length, expected);
            Assert.AreEqual(this.world.entitiesCapacity, expected);
        }
    }
}