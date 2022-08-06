namespace Morpeh.TestSuite.Editor {
    using NUnit.Framework;

    [TestFixture(Category = "Morpeh/SystemGroups")]
    public class SystemGroupTests {
        private const float DELTA_TIME = 0.1f;

        private World world;

        [SetUp]
        public void Setup() => this.world = World.Create();

        [TearDown]
        public void TearDown() {
            this.world.Dispose();
            this.world = null;
        }

        [Test]
        public void Systems_Disposed_On_SystemGroup_Removal() {
            var disposeCalls = 0;

            var system = new TestUpdateSystem {
                DisposeAction = f => ++disposeCalls,
            };

            var systemGroup = this.world.CreateSystemsGroup();
            systemGroup.AddSystem(system);
            this.world.AddSystemsGroup(0, systemGroup);

            this.world.Update(DELTA_TIME);

            this.world.RemoveSystemsGroup(systemGroup);

            Assert.AreEqual(1, disposeCalls);
        }
    }
}