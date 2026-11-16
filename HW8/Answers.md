# Часть 1. Задачи на понимание

## Задача 1. Анализ JSON-сериализации

### а) Что попадёт в JSON

Результат:

```json
{"Name":"Alex","Score":100}
```

- `Name` попадёт в JSON: это открытое свойство с публичным getter.
- `Score` попадёт в JSON по той же причине.
- `secretCode` не попадёт: это закрытое поле, которое сериализатор по умолчанию не обрабатывает.
- `IsAlive` не попадёт: это поле, а не свойство. По умолчанию `System.Text.Json` не включает даже открытые поля.

### б) Как сохранить private-поле в JSON

Можно оставить поле закрытым и добавить открытое свойство, возвращающее его значение. Атрибут `JsonPropertyName` задаёт имя ключа в JSON:

```csharp
using System.Text.Json.Serialization;

public class Player
{
    public string Name { get; set; } = "";
    public int Score { get; set; }
    private int secretCode = 42;
    public bool IsAlive;

    [JsonPropertyName("secretCode")]
    public int SecretCode => secretCode;
}
```

Теперь JSON содержит `"secretCode":42`, а само поле остаётся `private`. По умолчанию сериализуются и свойства только для чтения. Этот вариант решает именно задачу сериализации: для восстановления значения из JSON потребовался бы setter или конструктор.

### в) Как включить IsAlive: два способа

Причина отсутствия `IsAlive` — настройки сериализации полей по умолчанию.

**Способ 1:** включить все открытые поля через настройки:

```csharp
var options = new JsonSerializerOptions { IncludeFields = true };
string json = JsonSerializer.Serialize(p, options);
```

**Способ 2:** отметить только нужное открытое поле атрибутом:

```csharp
[JsonInclude]
public bool IsAlive;
```

Для атрибута нужно подключить `System.Text.Json.Serialization`. Ещё один возможный вариант — заменить поле свойством `public bool IsAlive { get; set; }`.

## Задача 2. Восстановление объекта из JSON

### а) Класс для десериализации

```csharp
public class Game
{
    public string? Title { get; set; }
    public int Year { get; set; }
    public double Rating { get; set; }
    public string[] Genres { get; set; } = Array.Empty<string>();
    public bool IsMultiplayer { get; set; }
}
```

Использование (в `json` находится строка из условия):

```csharp
Game? game = JsonSerializer.Deserialize<Game>(json);
```

Имена свойств совпадают с ключами JSON. `Year` — целое число, `Rating` — число с дробной частью, `Genres` — массив строк, `IsMultiplayer` — логическое значение. Все свойства имеют открытые getter и setter.

### б) Почему title не попадает в Title

При обычном вызове `JsonSerializer.Deserialize<T>()` сопоставление имён чувствительно к регистру. Поэтому `title` не соответствует `Title`: неизвестный ключ пропускается, а свойство остаётся со значением по умолчанию (`null` в приведённом классе).

Решение без переименования свойства:

```csharp
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
Game? game = JsonSerializer.Deserialize<Game>(json, options);
```

### в) Массив разных типов

`[1, "hello", true, null]` — валидный JSON. Элементы JSON-массива могут иметь разные типы, в том числе значение `null`.

Его удобно десериализовать в `JsonElement[]`:

```csharp
string json = "[1, \"hello\", true, null]";
JsonElement[] values = JsonSerializer.Deserialize<JsonElement[]>(json)!;

int number = values[0].GetInt32();
string? text = values[1].GetString();
bool flag = values[2].GetBoolean();
bool isNull = values[3].ValueKind == JsonValueKind.Null;
```

Здесь требуется `using System.Text.Json;`. Знак `!` уместен для этой заранее известной строки: её корень — массив, а не JSON `null`.

## Задача 3. File или FileStream

1. **Небольшой конфигурационный файл — `File`.** `File.ReadAllText(path)` читает весь текст за один вызов, после чего его можно вывести через `Console.WriteLine`.
2. **Автосохранение игрового прогресса — `File`.** Если сохранение небольшое и каждый раз записывается целиком, достаточно сериализовать объект и вызвать `File.WriteAllText`; для очень больших сохранений можно выбрать потоковую сериализацию через `FileStream`.
3. **Постоянное дописывание серверного лога — `FileStream` вместе со `StreamWriter`.** Поток открывается с `FileMode.Append`, а `StreamWriter` записывает текст; при частых записях поток можно держать открытым, не перечитывая многогигабайтный файл и не открывая его заново для каждой строки.
4. **Проверить наличие сохранения и прочитать его целиком — `File`.** Для этого подходят `File.Exists(path)` и затем `File.ReadAllText(path)`.
5. **Заменить 10 байт в середине большого бинарного файла — `FileStream`.** Нужно открыть существующий файл с `FileMode.Open` и `FileAccess.Write`, перейти к нужному смещению через `Seek` и записать 10 байт через `Write`, не перезаписывая остальные данные.

## Справочные материалы

- [Поля в System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/fields)
- [Сопоставление имён без учёта регистра](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/character-casing)
