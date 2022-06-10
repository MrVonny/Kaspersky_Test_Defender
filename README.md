# Тестовое Задание в Касперский
Привет! Понимаю, что разобраться во всех тестовых заданиях непростая работа, а потому постараюсь помочь разобраться в моём решении.

## Запуск
![Show](https://github.com/MrVonny/Kaspersky_Test_Defender/blob/master/Imgs/ShowRun.gif)
В папке Build содержатся два исполняемых файла. 
Сначала нужно запустить сервер:

    N:\Kaspersky\Build> scan_service

Затем в другой командой строке воспользоваться утилитой:

    N:\Kaspersky\Build> scan_util scan "N:\Program Files\dotnet"
    Scan task was created with ID: 1
    
    N:\Kaspersky\Build> scan_util status 1
    ====== Scan result ======
    Directory: N:\Program Files\dotnet
    Processed files: 13888
    JS detects: 0
    rm -rf detects: 0
    Rundll32 detects: 0
    Errors: 0
    Exection time: 00:00:05.5506241
    =========================

## Бенчмарки
Для определения наиболее эффективного метода обработки файлов проводились тесты с помощью бенчмарка [BenchmarkDotNet](https://benchmarkdotnet.org/articles/overview.html) на .NET SDK 6.0.300. Использовались следующие параметры:
- Способ обработки строк
- - NaiveScanner - метод string.Contains()
- - RegexScanner - Регулулярные выражения
- - AhoCorasickScanner - Алгоритм Ахо — Корасик
- Способ чтения файла
- - Построчно
- - Загружая в оператвиную память весь файл

Наиболее эффективно показал себя NaiveScanner с построчным чтением файла, обработав 2.2 Гб файлов за 2.66 сек.

|         Method |        FileScanner |         Mean |      Error |     StdDev |              Files |
|--------------- |------------------- |-------------:|-----------:|-----------:|------------------- |
| AllFileScanner | AhoCorasickScanner | 6,364.220 ms | 28.8487 ms | 25.5736 ms | 67683 (2258,33 MB) |
| ByLinesScanner | AhoCorasickScanner | 6,152.402 ms | 31.9289 ms | 28.3041 ms | 67683 (2258,33 MB) |
| AllFileScanner | NaiveScanner       | 3,675.439 ms | 35.6848 ms | 33.3796 ms | 67683 (2258,33 MB) |
| ByLinesScanner | NaiveScanner       | 2,664.581 ms | 50.9210 ms | 62.5356 ms | 67683 (2258,33 MB) |
| AllFileScanner | RegexScanner       | 4,258.040 ms | 42.4213 ms | 39.6809 ms | 67683 (2258,33 MB) |
| ByLinesScanner | RegexScanner       | 3,598.656 ms | 52.9781 ms | 49.5557 ms | 67683 (2258,33 MB) |
| AllFileScanner | AhoCorasickScanner | 1,226.064 ms | 14.9087 ms | 13.9457 ms |   2274 (477,08 MB) |
| ByLinesScanner | AhoCorasickScanner | 1,237.670 ms | 16.2449 ms | 15.1955 ms |   2274 (477,08 MB) |
| AllFileScanner | NaiveScanner       |   610.721 ms |  8.7563 ms |  7.3119 ms |   2274 (477,08 MB) |
| ByLinesScanner | NaiveScanner       |   473.914 ms |  5.9837 ms |  5.3044 ms |   2274 (477,08 MB) |
| AllFileScanner | RegexScanner       |   804.861 ms | 14.4512 ms | 13.5176 ms |   2274 (477,08 MB) |
| ByLinesScanner | RegexScanner       |   697.682 ms |  8.2260 ms |  7.2922 ms |   2274 (477,08 MB) |
| AllFileScanner | AhoCorasickScanner |    37.118 ms |  0.7323 ms |  1.7262 ms |      60 (27,19 MB) |
| ByLinesScanner | AhoCorasickScanner |    23.001 ms |  0.0528 ms |  0.0441 ms |      60 (27,19 MB) |
| AllFileScanner | NaiveScanner       |    22.544 ms |  0.8209 ms |  2.4075 ms |      60 (27,19 MB) |
| ByLinesScanner | NaiveScanner       |     8.211 ms |  0.1097 ms |  0.1026 ms |      60 (27,19 MB) |
| AllFileScanner | RegexScanner       |    31.940 ms |  0.6228 ms |  0.9129 ms |      60 (27,19 MB) |
| ByLinesScanner | RegexScanner       |    14.960 ms |  0.1336 ms |  0.1184 ms |      60 (27,19 MB) |

[Код бенмарков](/Defender.Tests.Benchmark/FileScanBenchmark.cs)
