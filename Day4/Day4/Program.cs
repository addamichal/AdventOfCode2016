using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            int result = 0;
            int storageSectorId = 0;
            var inputs = File.ReadAllLines("input.txt");
            foreach (var input in inputs)
            {
                var encryptedData = EncryptedData.Parse(input);

                var actualChecksum = GetActualChecksum(encryptedData.EncryptedName);

                if (encryptedData.Checksum == actualChecksum)
                    result += encryptedData.SectorId;

                var decryptString = DecryptString(encryptedData);
                if (decryptString.Contains("pole"))
                {
                    storageSectorId = encryptedData.SectorId;
                }
            }

            Console.WriteLine(result);
            Console.WriteLine(storageSectorId);
        }

        private static string DecryptString(EncryptedData encryptedData)
        {
            var decryptedString = "";
            var moveBy = encryptedData.SectorId % 26;
            foreach (var c in encryptedData.EncryptedName.ToCharArray())
            {
                if (c != '-')
                {
                    var movedC = c + moveBy;
                    if (movedC > 'z')
                        movedC = movedC - 26;

                    decryptedString += (char)movedC;
                }
                else
                {
                    decryptedString += " ";
                }
            }
            return decryptedString;
        }

        private static string GetActualChecksum(string encryptedName)
        {
            var selectedChars = encryptedName
                .Replace("-", "")
                .ToCharArray()
                .GroupBy(w => w)
                .ToDictionary(g => g.Key, g => g.Count())
                .OrderByDescending(w => w.Value)
                .ThenBy(w => w.Key)
                .Take(5)
                .Select(w => w.Key).ToArray();

            var actualChecksum = new string(selectedChars);
            return actualChecksum;
        }
    }

    public class EncryptedData
    {
        public string Checksum { get; set; }
        public int SectorId { get; set; }
        public string EncryptedName { get; set; }

        public static EncryptedData Parse(string input)
        {
            var checksumSubstring = input.Substring(input.IndexOf("["));
            var checksum = checksumSubstring.Substring(1, checksumSubstring.Length - 2);
            var inputWithoutChecksum = input.Substring(0, input.IndexOf("["));
            var encryptedName = inputWithoutChecksum.Substring(0, inputWithoutChecksum.LastIndexOf("-"));
            var sectorId = int.Parse(inputWithoutChecksum.Substring(inputWithoutChecksum.LastIndexOf("-") + 1));

            return new EncryptedData() { EncryptedName = encryptedName, Checksum = checksum, SectorId = sectorId };
        }
    }
}
