using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LevelDB;

namespace HealthDataTest.Models
{
    public class LevelDbHelper:Singleton<LevelDbHelper> {

        private string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db");
        private LevelDB.DB _db;


        public void Init() {
            _db = new DB(new Options() {
                CreateIfMissing = true
            }, _path);
        }

        public void Destroy() {
            _db.Close();
            _db.Dispose();
        }


        public string Get(string key) {
            return _db.Get(key);
        }

        public void Put(string key, string val) {
            _db.Put(key,val,new WriteOptions() {
                Sync = false
            });
        }

        public bool IsKeyExists(string key) {
            var val = _db.Get(key);
            return !string.IsNullOrEmpty(val);
        }

    }
}
