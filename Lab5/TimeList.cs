using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab5
{
    [Serializable]
    public class TimeList
    {
        private List<TimeItem> list = new List<TimeItem>();//закрытое поле типа List<TimeItem> - список объектов типа TimeItem
        public void Add(TimeItem ob) //открытый метод Add(TimeItem), добавляющий новый объект TimeItem к списку
        {
            list.Add(ob);
        }
        public void Load(string filename)//открытый метод для восстановления списка из файла с использованием сериализации
        {
            FileStream f = new FileStream(filename, FileMode.Open);
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (f.Length != 0)
                    list = (List<TimeItem>)bf.Deserialize(f);
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't load the file!");
                Console.WriteLine(e);
            }
            finally
            {
                f.Close();
            }
        }
        public void Save(string filename)//открытый метод для сохранения списка List<TimeItem> в файле с использованием сериализации
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream f = new FileStream(filename, FileMode.Open);
                bf.Serialize(f, list);
                f.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't save the file");
                Console.WriteLine(e);
            }
        }
        public override string ToString()
        {
            Console.WriteLine("коллекция из объекта TimeList");
            string temp = "Размерность\tЧисло повторов\tВремя C#\tВремя С++\tВо сколько раз отличается\n";
            int count = 1;
            foreach (TimeItem ob in list)
            {
                temp += count + ") " + ob + "\n";
                count++;
            }
            return temp;
        }
    }
}