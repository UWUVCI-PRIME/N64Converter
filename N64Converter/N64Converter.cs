namespace N64Converter
{
    public static class N64Convert
    {
        public static void Convert(string romPath, string outPath)
        {
            try
            {
                byte[] fullBytes = ReadFullBytes(romPath);
                byte[] bytes = ReadFirstBytes(romPath, 4);
                if (bytes == null)
                {
                    throw new Exception("Unable to Read File");
                }
                RomType romType = GetRomFormat(BitConverter.ToString(bytes));
                switch (romType)
                {
                    case RomType.Unknown:
                    default:
                        throw new Exception("Unkown File Type");
                    case RomType.z64:
                        //just copy file rom
                        break;
                    case RomType.n64:
                        fullBytes = N64toZ64(fullBytes);
                        break;
                    case RomType.v64:
                        fullBytes = V64toZ64(fullBytes);
                        break;
                }
                WriteBytesToPath(outPath, fullBytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        private static RomType GetRomFormat(string romHeader)
        {
            Console.WriteLine($"GetRomFormat: {romHeader}");
            switch (romHeader)
            {
                case "40-12-37-80":
                    return RomType.n64;
                case "80-37-12-40":
                    return RomType.z64;
                case "37-80-40-12":
                    return RomType.v64;
                default:
                    return RomType.Unknown;
            }
        }
        private static byte[] ReadFullBytes(string inputFilePath)
        {
            return File.ReadAllBytes(inputFilePath);
        }

        private static byte[] ReadFirstBytes(string inputFilePath, short bytesCount)
        {
            byte[] buffer = new byte[bytesCount];
            try
            {
                using (FileStream fs = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                {
                    var bytes_read = fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    if (bytes_read != buffer.Length)
                    {
                        throw new Exception("Error Reading File");
                    }
                    return buffer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static void WriteBytesToPath(string outputPathFile, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(outputPathFile, bytes);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static byte[] SwapWord(byte[] bytes, int a, int b)
        {
            byte temp = bytes[a];
            bytes[a] = bytes[b];
            bytes[b] = temp;
            //(bytes[a], bytes[b]) = (bytes[b], bytes[a]);

            return bytes;
        }

        private static byte[] V64toZ64(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i += 2)
                SwapWord(bytes, i, i + 1);

            return bytes;
        }

        private static byte[] N64toZ64(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i += 4)
            {
                SwapWord(bytes, i, i + 3);
                SwapWord(bytes, i + 1, i + 2);
            }

            return bytes;
        }

    }
}