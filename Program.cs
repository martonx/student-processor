using StudentProcessor;

var filePath = "Student.csv";
var fileRows = File.ReadLines(filePath);

var allStudents = new List<Student>();
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
    foreach (var student in allStudents.ToList())
    {
        if (student.Name == studentName)
        {
            //Megtaláltuk a diákot
            //Van-e olyan tantárgya, mint a csv sorban lévő tantárgy
            var subjectName = data[1];
            foreach (var subject in student.Subjects.ToList())
            {
                if (subject.Name == subjectName)
                {
                    //Megtaláltuk a tantárgyat
                    subject.Values.Add(Convert.ToInt32(data[2]));
                }
                else if (student.Subjects.Count() == 1)
                {
                    var newSubject = new Subject();
                    newSubject.Name = subjectName;
                    newSubject.Values = new List<int>();
                    newSubject.Values.Add(Convert.ToInt32(data[2]));
                    student.Subjects.Add(newSubject);
                }
            }
        }
        else if (allStudents.Count() == 1)
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
        }
    }
}

//Feladat: összegezzük a diákokat
//tantárgyakat
//írassuk ki az összegzett eredményeket
foreach (var student in allStudents)
{
    Console.WriteLine(student.Name);
}

//Adjunk új adatot a filehoz
// File.WriteAllText - felülírja a file tartalmát
// File.AppendAllText - nem bántja a meglévő adatainkat, csak hozzáadja az új szöveget a végéhez
File.AppendAllText(filePath, $"{Environment.NewLine}Gipsz Jakab,Matematika,5"); // egyben ad hozzá szöveget a file-hoz
File.AppendAllLines(filePath, new List<string> { $"{Environment.NewLine}Gipsz Jakab,Matematika,5" }); // soronként adja hozzá a string listát