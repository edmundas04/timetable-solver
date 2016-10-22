# Timetable solver

##Overview
### What is timetable solver?
Timetable solver is C# library for timetable optimization problem. At this point it optimizes timetable for **Teachers** and **Classes**
### How can I run this library?
You could use explanation in "Example" section. If you don't wont to work on example you could easily run sample project and follow instructions in console. It runs your selected optimization and exports "before" and "after" results to html in visual format.
## Example
This example will help you to construct first timetable solver and start optimization. It should be implementd in repository's sample project, because the project has some predifined logic which will help you build your data source and construct timetable object used by optimization. 
### Step 1. Constructing timetable
Firstly, you should get your datasource. It could be taken from any database, but at this moment we will build it using ```TimetableInfoBuilder```.

```csharp
var builder = new TimetableInfoBuilder();

var timetableDataSource = builder.AddClass(101, "Grade 5") // 101 - class id, "Grade 5"- class name
    .AddTeacher(201, "Albert", "Einstein") //201 - teacher id, "Albert" - first name, "Einstein" - last name
    .AddTeachingGroup(301, 4, "PH GR1", "Physics") //301 - teaching group id, 4 - number of lessons per week, "PH GR1" - teaching group name, "Physics" - subject name
    .AddClassAssignmentToTeachingGroup(101, 301) //101 - class id, 301 - teaching group id
    .AddTeacherAssignmentToTeachingGroup(201, 301) //201 - teacher id, 301 - teaching group id
    .AddAvailableWeekDay(DayOfWeek.Monday, 5) //"DayOfWeek.Monday" - day of week, 5 - number of lessons per day 
    .AddAvailableWeekDay(DayOfWeek.Tuesday, 5)
    .AddAvailableWeekDay(DayOfWeek.Wednesday, 5)
    .AddAvailableWeekDay(DayOfWeek.Thursday, 5)
    .AddAvailableWeekDay(DayOfWeek.Friday, 5)
    .Build();
```
Or you can create randomly generated datasource using static method ```TimetableInfoBuilder.GetRandomTimetableInfo```. When using this method you should provide number of classes to create, number of lessons for one class per week, number of lessons for teacher per week and number of available lesson spots in timetable per day
```csharp
var timetableDataSource = TimetableInfoBuilder.GetRandomTimetableInfo(50, 22, 20, 6);
```
Now datasource should be transformed into timetable object. We will use method ```ToTimetable``` on ```timetableDataSource```. This method transforms object type of ```TimetableInfo``` into object type of ```Timetable```.
```csharp
var timetable = timetableDataSource.ToTimetable();
```
### Step 2. Randomize timetable
```Randomizer``` is used to create primary version of timetable. It implements ```IGenerator``` interface, so the logic could be customized by needs. The ```Solver``` trys to improve primary version of timetable which is provided, it does not create one. Primary version could be made by any human or automatic algorithm. In this case it will be generated randomly.
```csharp
var randomizer = new Randomizer();
randomizer.Generate(timetable);
```
### Step 3. Create mutator
```Mutator``` is used by optimization algorithm to make changes in timetable. It implements ```IMutator``` interface, so the logic could be customized by needs. ```Mutator``` constructor accepts list of mutation operations. It defines what kind of changes will be performed during optimization.
```csharp
var mutations = new List<IMutation> { new Mutation(), new SwapMutation() };
var mutator = new Mutator(mutations);
```
In this case when method ```mutator.Mutate()``` is called it selects random mutation and performs timetable change according to its logic. ```Mutation``` object makes some random change in timetable, ```SwapMutation``` object takes two random timetable elements and swaps their times.
### Step 4. Create fitness claculator.
```FitnessCalculator``` is used by optimization algorithm to calculate quality of timetable. It implements ```IFitnessCalculator``` interface, so the logic could be customized by needs. ```FitnessCalculator``` constructor accepts penalty values for teacher timetable conflict, teacher timetable empty gap between lesssons, class timetable conflict, class timetable empty gap between lessons, class timetable empty gap till first lessson.**Conflict** in this case is the situation when teacher or class should be in two different lessons at the same time.
```csharp
var fitnessCalculator = new FitnessCalculator(100, 3, 100, 3, 1);
```
This is simple fitness calculator, but you should use ```CachedFitnessCalculator``` because it performs way better.
```csharp
var fitnessCalculator = new CachedFitnessCalculator(100, 3, 100, 3, 1);
```
### Step 5. Create solver
```Solver``` is optimization algorithm. It implements ```ISolver``` interface, so the logic could be customized by needs. ```Solver``` constructor accepts mutator, fitnessCalculator and timetable.
```csharp
var solver = new Solver(mutator, fitnessCalculator, timetable);
```
### Setp 5. Execute optimization
Now lets optimize timetable for 10 seconds.

```csharp
var optimization = solver.Solve();
Thread.Sleep(10000);
solver.Stop();
optimization.Wait();

var result = optimization.Result;
timetableDataSource.UpdateTimetableInfo(result);
```
After optimization the result is written back to ```timetableDataSource```. Now it's up to you what you want to do with the result.

Final code should look like this:
```csharp
var timetableDataSource = TimetableInfoBuilder.GetRandomTimetableInfo(50, 22, 20, 6);
var timetable = timetableDataSource.ToTimetable();

var randomizer = new Randomizer();
randomizer.Generate(timetable);

var mutations = new List<IMutation> { new Mutation(), new SwapMutation() };
var mutator = new Mutator(mutations);

var fitnessCalculator = new CachedFitnessCalculator(100, 3, 100, 3, 1);

var solver = new Solver(mutator, fitnessCalculator, timetable);

var optimization = solver.Solve();
Thread.Sleep(10000);
solver.Stop();
optimization.Wait();

var result = optimization.Result;
timetableDataSource.UpdateTimetableInfo(result);
```

Good luck :)
