# Тестовое Задание в Касперский


## Бенчмарки.
Бенчмарки проводились с помощью 

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
