using System;
using System.Text.Json;
namespace AlchimonAng.DB.Repository
{
    public interface ISaveLoader<T>
    {
        T Load(string path);
        void Save(T saveble, string path);
    }

    public class JsonLoader<T> : ISaveLoader<T>
    {
        public T Load(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                var resp = JsonSerializer.Deserialize<T>(json);
                if (resp is null) throw new Exception($"{path} NULL!!!");
                else return resp;
            }
        }

        public void Save(T saveble, string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                var json = JsonSerializer.Serialize(saveble);
                writer.WriteLine(json);
            }
        }
    }
}

