using N64Converter;

namespace ConverterTest
{  

    [TestClass]
    public class ConverterTest
    {
        private string _inRomN = @""; //N64 Rom
        private string _inRomV = @""; //V64 Rom
        private string _inRomZ = @""; //Z64 Rom

        private string _outRom = "out.z64";
        [TestMethod]
        public void TestConvertZ64toZ64()
        {
            N64Convert.Convert(_inRomZ, $"z{_outRom}");
            Assert.IsTrue(File.Exists($"z{_outRom}") );
        }

        [TestMethod]
        public void TestConvertV64toZ64()
        {
            N64Convert.Convert(_inRomZ, $"v{_outRom}");
            Assert.IsTrue(File.Exists($"v{_outRom}"));
        }

        [TestMethod]
        public void TestConvertN64toZ64()
        {
            N64Convert.Convert(_inRomZ, $"n{_outRom}");
            Assert.IsTrue(File.Exists($"n{_outRom}"));
        }
    }
}