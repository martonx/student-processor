using StudentProcessor;

var filePath = "Student.csv";
var fileRows = File.ReadLines(filePath);

var allStudents = new List<Student>();

//LINQ alapból mindig IEnumerable<valami> a gyűjtemény típusa, amit visszaad
//IEnumerable<string> names = allStudents.Select(student => student.Name);

// List - olyan gyűjtemény, amink szabadon módosíthatjuk (adhataunk hozzá, törölhetünk belőle) az elemeit 
// IEnumerable - meglévő listából hozzuk létre, elemeit létrehozás után nem módosíthatjuk (adhataunk hozzá, törölhetünk belőle)

//LINQ visszatérési értékét befolyásolni tudjuk .ToList / ToArray / ToDictionary / ToHashSet
// Array - alap gyűjtemény, inkább már csak historikus okokból maradt velünk
// List - ugyanazt tudja mint a tömb, DE könnyebb létrehozni, elemet hozzáadni, törölni
// Dictionary - Key, Value párokban tárolja az adatokat. Key egyedi kell, hogy legyen, illetve Key
//      alapján bináris keresést csinál -> nagyon gyors
// HashSet - hasonló a Dictionary-hez, de csak Key-ek vannak benne
//IEnumerable<string> names2 = allStudents.Select(student => student.Name).ToList();
//var names3 = new List<string>();
//foreach (var student in allStudents)
//{
//    names3.Add(student.Name);
//}
//return names3;

//bool containsAnyStudents = allStudents.Any();
//bool containsZsombor = allStudents.Any(student => student.Name == "Papdi Zsombor");
//bool result = false;
//foreach (var student in allStudents)
//{
//    if (student.Name == "Papdi Zsombor")
//      return true;
//}

//IEnumerable<Student> filteredStudents = allStudents.Where(student => student.Subjects.Count > 2);

//Single / First ha megtalálja, akkor visszaadja az adott elemet, különben hiba dobódik
//Student papdiZsombor = allStudents.Single(student => student.Name == "Papdi Zsombor");
//Student papdiZsombor2 = allStudents.First(student => student.Name == "Papdi Zsombor");

//SingleOrDefault / FirstOrDefault ha megtalálja, akkor visszaadja az adott elemet, különben hiba dobódik
//Student? papdiZsombor3 = allStudents.SingleOrDefault(student => student.Name == "Papdi Zsombor");
//Student? papdiZsombor4 = allStudents.FirstOrDefault(student => student.Name == "Papdi Zsombor");

//allStudents.IndexOf
//allStudents.Intersect
//allStudents.Distinct
//allStudents.DistinctBy - By esetekben megadhatjuk, hogy pontosan mi alapján történjen a művelet
foreach (var row in fileRows.Skip(1))
{
    var data = row.Split(',');
    var studentName = data[0];

    //1. eset amikor még üres az allStudents
    if (!allStudents.Any())
    {
        var student = new Student();
        
        student.Name = studentName;
        student.Subjects = new List<Subject>();
        var subject = new Subject();
        subject.Name = data[1];
        subject.Values = new List<int>();
        subject.Values.Add(Convert.ToInt32(data[2]));
        student.Subjects.Add(subject);
        allStudents.Add(student);

        continue;
    }

    //2. eset, mikor már nem üres allStudents
    //azaz meg kell keressük az adott csv
    //sorhoz tartozó studentet

    var existingStudent = allStudents.SingleOrDefault(x => x.Name == studentName);
    if (existingStudent is null)
    {
        var newStudent = new Student();

        newStudent.Name = studentName;
        newStudent.Subjects = new List<Subject>();
        var subject = new Subject();
        subject.Name = data[1];
        subject.Values = new List<int>();
        subject.Values.Add(Convert.ToInt32(data[2]));
        newStudent.Subjects.Add(subject);
        allStudents.Add(newStudent);

        continue;
    }

    //Diák létezik, elkezdjük megnézni a tanátrgyait
    var subjectName = data[1];
    var existingSubject = existingStudent.Subjects.SingleOrDefault(x => x.Name == subjectName);

    if (existingSubject is null)
    {
        var newSubject = new Subject();
        newSubject.Name = subjectName;
        newSubject.Values = new List<int>();
        newSubject.Values.Add(Convert.ToInt32(data[2]));
        existingStudent.Subjects.Add(newSubject);
    }
    else
    {
        existingSubject.Values.Add(Convert.ToInt32(data[2]));
    }
}

//(Házi)Feladat: Írassuk ki a diákoknak CSAK az irodalom jegyeit lehetőleg LINQ-t használva


//Adjunk új adatot a filehoz
// File.WriteAllText - felülírja a file tartalmát
// File.AppendAllText - nem bántja a meglévő adatainkat, csak hozzáadja az új szöveget a végéhez
File.AppendAllText(filePath, $"{Environment.NewLine}Gipsz Jakab,Matematika,5"); // egyben ad hozzá szöveget a file-hoz
File.AppendAllLines(filePath, new List<string> { $"{Environment.NewLine}Gipsz Jakab,Matematika,5" }); // soronként adja hozzá a string listát