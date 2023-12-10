using ShapeClassLibrary;

namespace TestTask
{
    class Program
    {
        public static string GetData(string file_name)
        {
            if (File.Exists(file_name))
            {
                using (var reader = new StreamReader(file_name))
                {
                    return reader.ReadToEnd();
                }   
            }
            else return String.Empty;
        }
        public static int GetIncongruityInStrings(string a, string b)
        {
            int minLength = Math.Min(a.Length, b.Length);

            for(int i= 0; i < minLength; i++)
            {
                if (a[i] != b[i]) return i;
            }

            if(a.Length != b.Length) return minLength;

            return -1;
        }
        public static List<string> CreateMistakeList(string data)
        {
            var stringCounter = 1;
            var mistakes = new List<string>();
            
            var strings = data.Split('\n');
            
            foreach (var item in strings)
            {
                if(String.Compare(item,"\r", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(item, "", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    stringCounter++;
                    continue;
                }
                var shapeCharacteristics = item.Trim().Split();

                var isShapeNameCorrect = true;

                var shapeName = shapeCharacteristics[0];
                var circleIndex = String.Compare(shapeName, "circle");
                var lineIndex = String.Compare("line", shapeName);
                var rectIndex = String.Compare("rect", shapeName);
                var pointIndex = String.Compare("point", shapeName);

                if (circleIndex != 0 && rectIndex != 0 && lineIndex != 0 && pointIndex != 0)
                {
                    mistakes.Add(item);
                    mistakes.Add(" ^");
                    var errorMessage = $"Введена неизвестная фигура, строка {stringCounter}";
                    mistakes.Add(errorMessage);
                    isShapeNameCorrect = false;
                }

                var lengthAll = shapeName.Length + 1;
                var characteristicCounter = 0;
                foreach (var characteristic in shapeCharacteristics) 
                { 
                    if(characteristicCounter != 0) 
                    {
                        int tryingResult;
                        var tryingParsing = int.TryParse(characteristic,out tryingResult);

                        if(!tryingParsing)
                        {
                            mistakes.Add(item);
                            mistakes.Add(new string(' ',lengthAll)+" ^");
                            var errorMessage = $"Неизвестный символ, строка {stringCounter}";
                            mistakes.Add(errorMessage);
                        }
                        else 
                        { 
                            if(shapeName == "circle" && characteristicCounter == 3 && tryingResult < 0)
                            {
                                mistakes.Add(item);
                                mistakes.Add(new string(' ', lengthAll) + " ^");
                                var errorMessage = $"Радиус окружности должен быть <= 0, строка {stringCounter}";
                                mistakes.Add(errorMessage);
                            }
                        }
                        lengthAll += characteristic.Length + 1;
                    }
                    characteristicCounter++;
                }

                if (isShapeNameCorrect)
                {
                    switch(shapeName)
                    {
                        case "circle":
                            {
                                var circleItems = 4;
                                if(shapeCharacteristics.Length != circleItems)
                                {
                                    mistakes.Add(item);
                                    mistakes.Add(String.Empty);
                                    var errorMessage = $"Не правильно описан круг [circle x y r], строка {stringCounter}";
                                    mistakes.Add(errorMessage);
                                }
                                break;
                            }
                        case "rect":
                            {
                                var rectItems = 5;
                                if (shapeCharacteristics.Length != rectItems)
                                {
                                    mistakes.Add(item);
                                    mistakes.Add(String.Empty);
                                    var errorMessage = $"Не правильно описан прямоугольник [rect x1 y1 x2 y2], строка {stringCounter}";
                                    mistakes.Add(errorMessage);
                                }
                                break;
                            }
                        case "line":
                            {
                                var lineItems = 5;
                                if (shapeCharacteristics.Length != lineItems)
                                {
                                    mistakes.Add(item);
                                    mistakes.Add(String.Empty);
                                    var errorMessage = $"Не правильно описана линия [line x1 y1 x2 y2], строка {stringCounter}";
                                    mistakes.Add(errorMessage);
                                }
                                break;
                            }
                        case "point":
                            {
                                var pointItems = 3;
                                if (shapeCharacteristics.Length != pointItems)
                                {
                                    mistakes.Add(item);
                                    mistakes.Add(String.Empty);
                                    var errorMessage = $"Не правильно описана точка [point x y], строка {stringCounter}";
                                    mistakes.Add(errorMessage);
                                }
                                break;
                            }
                    }
                }
                else
                {
                    var minShapeElementsCount = 3;
                    var maxShapeElementsCount = 5;
                    if (shapeCharacteristics.Length < minShapeElementsCount && shapeCharacteristics.Length > maxShapeElementsCount)
                    {
                        mistakes.Add(item);
                        mistakes.Add(String.Empty);
                        var errorMessage = $"Неверная структура строки, строка {stringCounter}";
                        mistakes.Add(errorMessage);
                    }
                }
                stringCounter++;
            }

            return mistakes;
        }
        public static List<Shape> CreateShapeStorage(string data)
        {
            var shapes = new List<Shape>();
            var infoAboutShapes = data.Split('\n');
            foreach(var item in infoAboutShapes) 
            {
                var shapeCharacteristics = item.Trim().Split();
                var shapeName = shapeCharacteristics[0];
                switch (shapeName)
                {
                    case "point":
                        {
                            var x = double.Parse(shapeCharacteristics[1]);
                            var y = double.Parse(shapeCharacteristics[2]);
                            var shapePoints = new List<(double, double)>();
                            shapePoints.Add((x, y));
                            var point = new Point(shapePoints);
                            shapes.Add(point); 
                            break;
                        }
                    case "rect":
                        {
                            var shapePoints = new List<(double, double)>();
                            for (int i = 1; i < shapeCharacteristics.Length; i+=2)
                            {
                                var x = double.Parse(shapeCharacteristics[i]);
                                var y = double.Parse(shapeCharacteristics[i+1]);
                                shapePoints.Add((x, y));
                            }
                            var rect = new Rect(shapePoints);
                            shapes.Add(rect);
                            break;
                        }
                    case "line":
                        {
                            var shapePoints = new List<(double, double)>();
                            for (int i = 1; i < shapeCharacteristics.Length; i+=2)
                            {
                                var x = double.Parse(shapeCharacteristics[i]);
                                var y = double.Parse(shapeCharacteristics[i+1]);
                                shapePoints.Add((x, y));
                            }
                            var line = new Line(shapePoints);
                            shapes.Add(line);
                            break;
                        }
                    case "circle":
                        {
                            var shapePoints = new List<(double, double)>();
                            var x = double.Parse(shapeCharacteristics[1]);
                            var y = double.Parse(shapeCharacteristics[2]);
                            var radius = int.Parse(shapeCharacteristics[3]);
                            shapePoints.Add((x, y));
                            var circle = new Circle(shapePoints,radius);
                            shapes.Add(circle);
                            break;
                        }
                }
                
            }
            return shapes;
        }
        public static void Main()
        {
            var fileName = "example.txt";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var data = GetData(filePath);
            
            var mistakeList = CreateMistakeList(data);
            if(mistakeList.Count > 0)
            {
                foreach(var mistake in mistakeList)
                {
                    Console.WriteLine(mistake);
                }
            }
            else
            {
                var shapes = CreateShapeStorage(data);

                foreach (var shape in shapes)
                {
                    shape.Draw();
                }

                for (int i = 0; i < shapes.Count - 1; i++)
                {
                    shapes[i].Intersect(shapes[i + 1]);
                }
            }
            
        }
    }
}
