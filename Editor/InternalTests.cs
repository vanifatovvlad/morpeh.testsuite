namespace Morpeh.TestSuite.Editor {
  using NUnit.Framework;
  using UnityEngine.TestTools;

  [TestFixture(Category = "Morpeh/Entity")]
  public class EntityInternalTests {
      private World world;

      [SetUp]
      public void Setup() => this.world = World.Create();

      [TearDown]
      public void TearDown() {
          this.world.Dispose();
          this.world = null;
      }
      
      [Test]
      public void Check() {
      }

    }
}