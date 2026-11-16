### Задание №1

**а)**

Вот что выведет:

```json
{"Name":"Alex","Score":100}
```

Name: попадёт в json, потому что это публичное свойство с { get; set; }

Score: попадёт в json, потому что это публичное свойство с { get; set; }

secretCode: не попадёт, потому что это приватное поле

IsAlive: не попадёт, потому что это обычное поле, а JsonSerializer по умолчанию сериализует публичные свойства

**б)**

Нужно добавить публичное свойство с get:

```csharp
private int secretCode = 42;

public int SecretCode
{
    get { return secretCode; }
}
```

Поле останется private, а его значение попадёт в json через свойство SecretCode

**в)**

Поле IsAlive не попадает, потому что JsonSerializer по умолчанию не сериализует поля

**1 способ**

Сделать поле свойством:

```csharp
public bool IsAlive { get; set; }
```

**2 способ**

Добавить [JsonInclude]:

```csharp
using System.Text.Json.Serialization;

public class Player
{
    [JsonInclude]
    public bool IsAlive;
}
```

### Задание №2

**а)**

```csharp
public class Game
{
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public string[] Genres { get; set; }
    public bool IsMultiplayer { get; set; }
}
```

**б)**

Десериализация не работает правильно, потому что JsonSerializer по умолчанию учитывает регистр букв Для него Title и title — разные имена, поэтому Title остаётся null

Нужно отключить проверку регистра:

```csharp
JsonSerializerOptions options = new()
{
    PropertyNameCaseInsensitive = true
};

Game game = JsonSerializer.Deserialize<Game>(json, options);
```

Для JsonSerializer нужно подключить `using System.Text.Json;`

**в)**

Да, это правильный json В одном массиве могут быть числа, строки, логические значения и null

Можно записать его в массив JsonElement, потому что у элементов разные типы:

```csharp
string json = "[1, \"hello\", true, null]";
JsonElement[] items = JsonSerializer.Deserialize<JsonElement[]>(json);
```

Например, первое число можно получить через `items[0].GetInt32()`, а строку — через `items[1].GetString()`

### Задание №3

**1) File**

Файл маленький, поэтому можно прочитать его целиком через File.ReadAllText и вывести на экран

**2) File**

Для небольшого сохранения хватит File.WriteAllText Сначала переводим прогресс в json, потом записываем его в файл при каждом автосохранении

**3) FileStream**

Для частых записей лучше открыть поток в режиме FileMode.Append и дописывать строки через StreamWriter Тогда не придётся каждый раз открывать большой файл заново, а старые записи сохранятся

**4) File**

Сначала проверяем наличие файла через File.Exists Если файл есть, читаем его целиком через File.ReadAllText

**5) FileStream**

Через Seek можно перейти к нужному месту файла и записать 10 байт через Write Остальную часть файла перезаписывать не нужно
