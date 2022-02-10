using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService  {

	private SQLiteConnection _connection;

	public DataService(string DatabaseName){

#if UNITY_EDITOR
		var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
			var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
			var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#else
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);


#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif

            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);     

	}

	public void CreateDB(){
		_connection.DropTable<category> ();
		_connection.DropTable<puzzleItem> ();
		_connection.CreateTable<category> ();
		_connection.CreateTable<puzzleItem> ();

		createCategories (_connection);
		createAnimals (_connection);
	}


	#region createCategories
	public void createCategories(SQLiteConnection _connection) {
		_connection.InsertAll (new[]{
			new category {
				Id = 1,
				Name="Animals"
			},
			new category {
				Id = 2,
				Name="Body"
			},
			new category {
				Id = 3,
				Name="Buildings"
			},
			new category {
				Id = 4,
				Name="Business"
			},
			new category {
				Id = 5,
				Name="Cities"
			},
			new category {
				Id = 6,
				Name="Cats"
			},
			new category {
				Id = 7,
				Name="Clothing"
			},
			new category {
				Id = 8,
				Name="Countries"
			},
			new category {
				Id = 9,
				Name="Flowers"
			},
			new category {
				Id = 10,
				Name="Foods"
			},
			new category {
				Id = 11,
				Name="Jobs"
			},
			new category {
				Id = 12,
				Name="Music"
			},
			new category {
				Id = 13,
				Name="Science"
			},
			new category {
				Id = 14,
				Name="Sport"
			},
			new category {
				Id = 15,
				Name="Fruits"
			},
			new category {
				Id = 16,
				Name="Vehicles"
			}
		});
	}
	#endregion


	#region Animals
	public void createAnimals(SQLiteConnection _connection) {
		_connection.InsertAll (new[]{
			
			// Animals
			// Level 1
			new puzzleItem {
				Id = 1,
				CategoryId = 1,
				Level = 1,
				ItemName = "Ant",
			},
			new puzzleItem {
				Id = 2,
				CategoryId = 1,
				Level = 1,
				ItemName = "Buffalo",
			},
			new puzzleItem {
				Id = 3,
				CategoryId = 1,
				Level = 1,
				ItemName = "Cheetah",
			},
			new puzzleItem {
				Id = 4,
				CategoryId = 1,
				Level = 1,
				ItemName = "Chimpanzee",
			},
			new puzzleItem {
				Id = 5,
				CategoryId = 1,
				Level = 1,
				ItemName = "Cockroach",
			},
			new puzzleItem {
				Id = 6,
				CategoryId = 1,
				Level = 1,
				ItemName = "Deer",
			},
			new puzzleItem {
				Id = 7,
				CategoryId = 1,
				Level = 1,
				ItemName = "Falcon",
			},
			new puzzleItem {
				Id = 8,
				CategoryId = 1,
				Level = 1,
				ItemName = "Jaguar",
			},
			new puzzleItem {
				Id = 9,
				CategoryId = 1,
				Level = 1,
				ItemName = "Leopard",
			},
			new puzzleItem {
				Id = 10,
				CategoryId = 1,
				Level = 1,
				ItemName = "Monkey",
			},
			new puzzleItem {
				Id = 11,
				CategoryId = 1,
				Level = 1,
				ItemName = "Narwhal",
			},
			new puzzleItem {
				Id = 12,
				CategoryId = 1,
				Level = 1,
				ItemName = "Pelican",
			},
			new puzzleItem {
				Id = 13,
				CategoryId = 1,
				Level = 1,
				ItemName = "Penguin",
			},
			new puzzleItem {
				Id = 14,
				CategoryId = 1,
				Level = 1,
				ItemName = "Pig",
			},
			new puzzleItem {
				Id = 15,
				CategoryId = 1,
				Level = 1,
				ItemName = "Rabbit",
			},
			new puzzleItem {
				Id = 16,
				CategoryId = 1,
				Level = 1,
				ItemName = "Rat",
			},
			new puzzleItem {
				Id = 17,
				CategoryId = 1,
				Level = 1,
				ItemName = "Salmon",
			},
			new puzzleItem {
				Id = 18,
				CategoryId = 1,
				Level = 1,
				ItemName = "Turkey",
			},
			new puzzleItem {
				Id = 19,
				CategoryId = 1,
				Level = 1,
				ItemName = "Wolf",
			},
			new puzzleItem {
				Id = 20,
				CategoryId = 1,
				Level = 1,
				ItemName = "Worm",
			},
			
			//Level 2
			
			new puzzleItem {
				Id = 21,
				CategoryId = 1,
				Level = 2,
				ItemName = "Antelope",
			},
			new puzzleItem {
				Id = 22,
				CategoryId = 1,
				Level = 2,
				ItemName = "Ape",
			},
			new puzzleItem {
				Id = 23,
				CategoryId = 1,
				Level = 2,
				ItemName = "Bat",
			},
			new puzzleItem {
				Id = 24,
				CategoryId = 1,
				Level = 2,
				ItemName = "Gorilla",
			},
			new puzzleItem {
				Id = 25,
				CategoryId = 1,
				Level = 2,
				ItemName = "Hamster",
			},
			new puzzleItem {
				Id = 26,
				CategoryId = 1,
				Level = 2,
				ItemName = "Octopus",
			},
			new puzzleItem {
				Id = 27,
				CategoryId = 1,
				Level = 2,
				ItemName = "Pheasant",
			},
			new puzzleItem {
				Id = 28,
				CategoryId = 1,
				Level = 2,
				ItemName = "Porcupine",
			},
			new puzzleItem {
				Id = 29,
				CategoryId = 1,
				Level = 2,
				ItemName = "Sardine",
			},
			new puzzleItem {
				Id = 30,
				CategoryId = 1,
				Level = 2,
				ItemName = "Seahorse",
			},
			new puzzleItem {
				Id = 31,
				CategoryId = 1,
				Level = 2,
				ItemName = "Wallaby",
			},
			new puzzleItem {
				Id = 32,
				CategoryId = 1,
				Level = 2,
				ItemName = "Wasp",
			},
			new puzzleItem {
				Id = 33,
				CategoryId = 1,
				Level = 2,
				ItemName = "Whale",
			},
			new puzzleItem {
				Id = 34,
				CategoryId = 1,
				Level = 2,
				ItemName = "Yak",
			},
			new puzzleItem {
				Id = 35,
				CategoryId = 1,
				Level = 2,
				ItemName = "Mole",
			},
			new puzzleItem {
				Id = 36,
				CategoryId = 1,
				Level = 2,
				ItemName = "Owl",
			},
			new puzzleItem {
				Id = 37,
				CategoryId = 1,
				Level = 2,
				ItemName = "Koala",
			},
			new puzzleItem {
				Id = 38,
				CategoryId = 1,
				Level = 2,
				ItemName = "Gnu",
			},
			new puzzleItem {
				Id = 39,
				CategoryId = 1,
				Level = 2,
				ItemName = "Frog",
			},
			new puzzleItem {
				Id = 40,
				CategoryId = 1,
				Level = 2,
				ItemName = "Fox",
			},

			// Level 3

			new puzzleItem {
				Id = 41,
				CategoryId = 1,
				Level = 3,
				ItemName = "Anteater",
			},
			new puzzleItem {
				Id = 42,
				CategoryId = 1,
				Level = 3,
				ItemName = "Beaver",
			},
			new puzzleItem {
				Id = 43,
				CategoryId = 1,
				Level = 3,
				ItemName = "Caribou",
			},
			new puzzleItem {
				Id = 44,
				CategoryId = 1,
				Level = 3,
				ItemName = "Coyote",
			},
			new puzzleItem {
				Id = 45,
				CategoryId = 1,
				Level = 3,
				ItemName = "Duck",
			},
			new puzzleItem {
				Id = 46,
				CategoryId = 1,
				Level = 3,
				ItemName = "Flamingo",
			},
			new puzzleItem {
				Id = 47,
				CategoryId = 1,
				Level = 3,
				ItemName = "Giraffe",
			},
			new puzzleItem {
				Id = 48,
				CategoryId = 1,
				Level = 3,
				ItemName = "Goldfinch",
			},
			new puzzleItem {
				Id = 49,
				CategoryId = 1,
				Level = 3,
				ItemName = "Goose",
			},
			new puzzleItem {
				Id = 50,
				CategoryId = 1,
				Level = 3,
				ItemName = "Guineafowl",
			},
			new puzzleItem {
				Id = 51,
				CategoryId = 1,
				Level = 3,
				ItemName = "Lyrebird",
			},
			new puzzleItem {
				Id = 52,
				CategoryId = 1,
				Level = 3,
				ItemName = "Opossum",
			},
			new puzzleItem {
				Id = 53,
				CategoryId = 1,
				Level = 3,
				ItemName = "Rhinoceros",
			},
			new puzzleItem {
				Id = 54,
				CategoryId = 1,
				Level = 3,
				ItemName = "Rook",
			},
			new puzzleItem {
				Id = 55,
				CategoryId = 1,
				Level = 3,
				ItemName = "Sealion",
			},
			new puzzleItem {
				Id = 56,
				CategoryId = 1,
				Level = 3,
				ItemName = "Wolverine",
			},
			new puzzleItem {
				Id = 57,
				CategoryId = 1,
				Level = 3,
				ItemName = "Dragonfly",
			},
			new puzzleItem {
				Id = 58,
				CategoryId = 1,
				Level = 3,
				ItemName = "Butterfly",
			},
			new puzzleItem {
				Id = 59,
				CategoryId = 1,
				Level = 3,
				ItemName = "Kangaroo",
			},
			new puzzleItem {
				Id = 60,
				CategoryId = 1,
				Level = 3,
				ItemName = "Woodcock",
			},
			new puzzleItem {
				Id = 61,
				CategoryId = 2,
				Level = 1,
				ItemName = "Sandra",
			},
			new puzzleItem {
				Id = 62,
				CategoryId = 2,
				Level = 1,
				ItemName = "Jens",
			},
			new puzzleItem {
				Id = 63,
				CategoryId = 2,
				Level = 2,
				ItemName = "Werner",
			}
		});
	}
	#endregion

	//
	// public IEnumerable<category> getCategories()
	//
	// This method returns a list of categories
	//

	public IEnumerable<category> getCategories(){
		return _connection.Table<category>();
	}


	//
	// public int getCategoriesCount() 
	//
	// This method counts the number of categories 
	// and returns a scalar value (int)
	// 

	public int getCategoriesCount() {
		return _connection.ExecuteScalar<int> ("select count(*) from category");
	}



	//
	// public category getCategory(int ID)
	//
	// This method get the category according to the ID
	//

	public category getCategory(int ID) {
		return _connection.Table<category>().Where(x => x.Id == ID).FirstOrDefault();
	}



	//
	// public IEnumerable<puzzleItem> getItems(int level, int category)
	//
	// This method gets all items belonging to a level and a certain category
	//

	public IEnumerable<puzzleItem> getItems(int level, int category) {
		// Query in standard SQL syntax
		//return _connection.Query<puzzleItem> ("Select * from puzzleItem Where Level=? and CategoryId=?", level, category);
		// Query using LINQ
		return _connection.Table<puzzleItem> ().Where (x => x.Level == level && x.CategoryId == category);
	}


	// Demo methods
	/*
	public IEnumerable<Person> GetPersonsNamedRoberto(){
		return _connection.Table<Person>().Where(x => x.Name == "Roberto");
	}

	public Person GetJohnny(){
		return _connection.Table<Person>().Where(x => x.Name == "Johnny").FirstOrDefault();
	}

	public Person CreatePerson(){
		var p = new Person{
				Name = "Johnny",
				Surname = "Mnemonic",
				Age = 21
		};
		_connection.Insert (p);
		return p;
	}
	*/
}
