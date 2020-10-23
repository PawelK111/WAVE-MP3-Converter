using NAudio.Wave;
using NAudio.Lame;
using System.Threading.Tasks;

namespace MP3_WAVEConverter
{
    public static class FileConverter
    {
        public static async Task ToWave(string MP3FileName, string WaveFileName)
        {
            await Task.Factory.StartNew(() =>
            {
                using (Mp3FileReader mp3 = new Mp3FileReader(MP3FileName))
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                    WaveFileWriter.CreateWaveFile(WaveFileName, pcm);
            });
        }
        public static async Task ToMP3(string WaveFileName, string MP3FileName, int bitRate)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var reader = new AudioFileReader(WaveFileName))
                using (var writer = new LameMP3FileWriter(MP3FileName, reader.WaveFormat, bitRate))
                    reader.CopyTo(writer);
            });
        }
    }
}
