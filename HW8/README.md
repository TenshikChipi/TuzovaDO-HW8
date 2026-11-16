# ДЗ8

Задания 1–3 находятся в [Answers.md](Answers.md).

Задания 4, 5 и 6 сделаны отдельными консольными проектами. Для запуска нужен .NET 7.

Из корня репозитория:

```sh
dotnet build HW8/HW8.sln
dotnet run --project HW8/Task4
dotnet run --project HW8/Task5
dotnet run --project HW8/Task6
```

В задании 5 файл save.json создаётся рядом с программой, в папке Task5/bin/Debug/net7.0.

В задании 6 при первом запуске вводим настройки. При втором запуске программа читает settings.json из папки Task6/bin/Debug/net7.0 и показывает сохранённые значения.
