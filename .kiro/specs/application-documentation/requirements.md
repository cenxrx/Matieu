# Requirements Document

## Introduction

Данный документ описывает требования к разработке полной документации для приложения Matieu — системы управления услугами салона красоты. Приложение разработано на платформе Avalonia UI (.NET 8) с использованием PostgreSQL в качестве базы данных. Документация должна включать расширенное руководство пользователя и полную техническую спецификацию приложения.

## Glossary

- **Matieu**: Desktop приложение для управления услугами салона красоты
- **Documentation_System**: Система документации, включающая руководство пользователя и техническую спецификацию
- **User_Manual**: Расширенное руководство пользователя с детальным описанием функций и процессов
- **Technical_Specification**: Полная техническая спецификация приложения
- **Process_Indication**: Описание индикации процессов (статусы, ошибки, прогресс)
- **Function_Description**: Детальное описание каждой функции приложения
- **Role**: Роль пользователя в системе (пользователь, модератор, администратор, мастер)
- **Service**: Услуга салона красоты (категории: Кастом, Косплей)
- **Collection**: Коллекция услуг
- **Pagination**: Постраничный вывод услуг (3 на страницу)
- **Screenshot**: Графическая иллюстрация интерфейса приложения
- **Test_Case**: Тест-кейс для проверки функционала
- **Unit_Test**: Модульный тест кода

## Requirements

### Requirement 1: Структура расширенного руководства пользователя

**User Story:** Как пользователь документации, я хочу иметь структурированное руководство пользователя, чтобы быстро находить информацию о функциях приложения.

#### Acceptance Criteria

1. THE User_Manual SHALL содержать раздел "Содержание по функциям" с детальным описанием каждой функции
2. THE User_Manual SHALL содержать раздел "Содержание по индикации процесса" с описанием всех статусов, ошибок и индикаторов прогресса
3. THE User_Manual SHALL содержать раздел "Графические иллюстрации" со ссылками на скриншоты и их описанием
4. THE User_Manual SHALL содержать раздел "Роли пользователей" с описанием прав доступа для каждой роли
5. THE User_Manual SHALL быть написан на русском языке

### Requirement 2: Описание функций в руководстве пользователя

**User Story:** Как пользователь приложения, я хочу видеть пошаговые инструкции для каждой операции, чтобы правильно использовать функционал.

#### Acceptance Criteria

1. FOR ALL функций приложения, THE User_Manual SHALL содержать пошаговые инструкции по их использованию
2. WHEN описывается функция авторизации, THE User_Manual SHALL включать описание всех тестовых учётных записей
3. WHEN описывается функция просмотра услуг, THE User_Manual SHALL включать описание пагинации (3 услуги на страницу)
4. WHEN описывается функция поиска, THE User_Manual SHALL включать описание поиска по названию
5. WHEN описывается функция фильтрации, THE User_Manual SHALL включать описание фильтрации по коллекции
6. WHEN описывается функция добавления услуги, THE User_Manual SHALL указывать что функция доступна только модератору и администратору
7. WHEN описывается функция редактирования услуги, THE User_Manual SHALL указывать что функция доступна только модератору и администратору
8. WHEN описывается функция редактирования услуги, THE User_Manual SHALL включать описание автоматического обновления времени изменения
9. WHEN описывается функция закрытия приложения, THE User_Manual SHALL включать описание диалога подтверждения

### Requirement 3: Индикация процессов в руководстве пользователя

**User Story:** Как пользователь приложения, я хочу понимать значение всех индикаторов и сообщений, чтобы правильно интерпретировать состояние системы.

#### Acceptance Criteria

1. THE User_Manual SHALL содержать описание всех сообщений об ошибках с указанием их причин
2. THE User_Manual SHALL содержать описание индикатора пагинации (формат "X-Y из Z")
3. THE User_Manual SHALL содержать описание визуальной индикации ошибок (красный текст под полями)
4. THE User_Manual SHALL содержать описание отображения даты и времени последнего изменения услуги
5. THE User_Manual SHALL содержать описание состояний кнопок навигации (активна/неактивна)

### Requirement 4: Графические иллюстрации в руководстве пользователя

**User Story:** Как пользователь документации, я хочу видеть скриншоты интерфейса, чтобы визуально понимать описываемые функции.

#### Acceptance Criteria

1. FOR ALL основных окон приложения, THE User_Manual SHALL содержать ссылки на скриншоты в формате "Рисунок X.X"
2. THE User_Manual SHALL содержать ссылку на скриншот окна авторизации
3. THE User_Manual SHALL содержать ссылку на скриншот главного окна со списком услуг
4. THE User_Manual SHALL содержать ссылку на скриншот окна добавления/редактирования услуги
5. THE User_Manual SHALL содержать ссылку на скриншот диалога подтверждения закрытия
6. FOR ALL ссылок на скриншоты, THE User_Manual SHALL содержать текстовое описание изображения

### Requirement 5: Описание ролей пользователей

**User Story:** Как администратор системы, я хочу понимать права доступа каждой роли, чтобы правильно назначать роли пользователям.

#### Acceptance Criteria

1. THE User_Manual SHALL содержать описание роли "Пользователь" с перечислением доступных функций
2. THE User_Manual SHALL содержать описание роли "Модератор" с перечислением доступных функций
3. THE User_Manual SHALL содержать описание роли "Администратор" с перечислением доступных функций
4. THE User_Manual SHALL содержать описание роли "Мастер" с перечислением доступных функций
5. FOR ALL ролей, THE User_Manual SHALL указывать какие функции доступны только этой роли

### Requirement 6: Структура технической спецификации

**User Story:** Как разработчик или технический специалист, я хочу иметь полную техническую спецификацию приложения, чтобы понимать его архитектуру и технологии.

#### Acceptance Criteria

1. THE Technical_Specification SHALL содержать раздел "Информация о разработчике"
2. THE Technical_Specification SHALL содержать раздел "Информация о сервере баз данных"
3. THE Technical_Specification SHALL содержать раздел "Информация о базе данных"
4. THE Technical_Specification SHALL содержать раздел "Информация о платформе"
5. THE Technical_Specification SHALL содержать раздел "Информация о программе"
6. THE Technical_Specification SHALL содержать раздел "Результаты тестирования"
7. THE Technical_Specification SHALL быть написана на русском языке

### Requirement 7: Информация о сервере баз данных

**User Story:** Как системный администратор, я хочу знать параметры подключения к базе данных, чтобы настроить окружение.

#### Acceptance Criteria

1. THE Technical_Specification SHALL указывать СУБД (PostgreSQL 17)
2. THE Technical_Specification SHALL указывать хост (localhost)
3. THE Technical_Specification SHALL указывать порт (5432)
4. THE Technical_Specification SHALL указывать имя пользователя базы данных (postgres)
5. THE Technical_Specification SHALL содержать строку подключения из файла Database.cs

### Requirement 8: Информация о базе данных

**User Story:** Как разработчик, я хочу понимать структуру базы данных, чтобы работать с данными приложения.

#### Acceptance Criteria

1. THE Technical_Specification SHALL указывать название базы данных (matieu)
2. THE Technical_Specification SHALL указывать кодировку базы данных (UTF-8)
3. THE Technical_Specification SHALL содержать список всех таблиц (roles, collections, users, masters, services, master_services, appointments, reviews, qualification_requests)
4. THE Technical_Specification SHALL указывать нормальную форму базы данных (3НФ)
5. THE Technical_Specification SHALL содержать описание структуры таблиц с полями и типами данных
6. THE Technical_Specification SHALL содержать описание связей между таблицами (внешние ключи)
7. THE Technical_Specification SHALL содержать описание первичных ключей (SERIAL с автоинкрементом)

### Requirement 9: Информация о платформе

**User Story:** Как разработчик, я хочу знать технологический стек приложения, чтобы настроить среду разработки.

#### Acceptance Criteria

1. THE Technical_Specification SHALL указывать операционную систему (Windows)
2. THE Technical_Specification SHALL указывать версию .NET (8.0)
3. THE Technical_Specification SHALL указывать UI фреймворк (Avalonia UI 11.3.6)
4. THE Technical_Specification SHALL указывать тип приложения (Desktop WinExe)
5. THE Technical_Specification SHALL содержать список всех NuGet пакетов с версиями из файла Matieu.csproj

### Requirement 10: Информация о программе

**User Story:** Как разработчик, я хочу понимать архитектуру и функционал приложения, чтобы поддерживать и развивать его.

#### Acceptance Criteria

1. THE Technical_Specification SHALL указывать название приложения (Матье)
2. THE Technical_Specification SHALL указывать версию приложения
3. THE Technical_Specification SHALL указывать язык программирования (C#)
4. THE Technical_Specification SHALL указывать стандарт кодирования (camelCase)
5. THE Technical_Specification SHALL содержать описание структуры проекта (Views, Models, Data, Resources)
6. THE Technical_Specification SHALL содержать полный список функционала приложения
7. THE Technical_Specification SHALL содержать описание архитектуры приложения (паттерны, подходы)
8. THE Technical_Specification SHALL содержать описание моделей данных (Service, Collection, User)
9. THE Technical_Specification SHALL содержать описание всех окон приложения (LoginWindow, MainWindow, EditServiceWindow, ConfirmDialog)

### Requirement 11: Описание функционала приложения

**User Story:** Как технический специалист, я хочу видеть полный список функций приложения, чтобы оценить его возможности.

#### Acceptance Criteria

1. THE Technical_Specification SHALL содержать описание функции авторизации с указанием ролей
2. THE Technical_Specification SHALL содержать описание функции просмотра услуг с пагинацией (3 на страницу)
3. THE Technical_Specification SHALL содержать описание функции сортировки по алфавиту
4. THE Technical_Specification SHALL содержать описание функции поиска по названию
5. THE Technical_Specification SHALL содержать описание функции фильтрации по коллекции
6. THE Technical_Specification SHALL содержать описание категорий услуг (Кастом, Косплей)
7. THE Technical_Specification SHALL содержать описание функции добавления услуги (только модератор/администратор)
8. THE Technical_Specification SHALL содержать описание функции редактирования услуги (только модератор/администратор)
9. THE Technical_Specification SHALL содержать описание автоматического обновления времени изменения (updated_at)
10. THE Technical_Specification SHALL содержать описание диалога подтверждения при закрытии
11. THE Technical_Specification SHALL содержать описание кастомных кнопок управления окном (свернуть/закрыть)

### Requirement 12: Результаты тестирования

**User Story:** Как менеджер проекта, я хочу видеть результаты тестирования, чтобы оценить качество приложения.

#### Acceptance Criteria

1. THE Technical_Specification SHALL содержать описание unit тестов из файла ServiceUpdateTimeTests.cs
2. THE Technical_Specification SHALL указывать количество unit тестов и их статус (5 тестов, все пройдены)
3. THE Technical_Specification SHALL содержать описание тест-кейсов из файла Тест_кейсы.txt
4. THE Technical_Specification SHALL указывать количество тест-кейсов и их статус (25 кейсов, все пройдены)
5. THE Technical_Specification SHALL указывать количество ошибок при сборке (0)
6. THE Technical_Specification SHALL содержать таблицу с результатами тестирования по функциям

### Requirement 13: Формат документации

**User Story:** Как пользователь документации, я хочу чтобы документы были структурированы и легко читались, чтобы быстро находить нужную информацию.

#### Acceptance Criteria

1. THE User_Manual SHALL следовать структуре примера из файла Ruc.txt
2. THE User_Manual SHALL использовать нумерацию разделов
3. THE User_Manual SHALL использовать ссылки на рисунки в формате "Рисунок X.X"
4. THE Technical_Specification SHALL использовать разделители между секциями
5. THE Technical_Specification SHALL использовать заголовки в верхнем регистре для основных разделов
6. WHEN документ содержит код, THE Documentation_System SHALL использовать форматирование кода
7. WHEN документ содержит таблицы, THE Documentation_System SHALL использовать табличный формат

### Requirement 14: Полнота документации

**User Story:** Как новый пользователь приложения, я хочу чтобы документация покрывала все аспекты работы с приложением, чтобы не обращаться за дополнительной помощью.

#### Acceptance Criteria

1. THE Documentation_System SHALL включать описание всех окон приложения
2. THE Documentation_System SHALL включать описание всех функций приложения
3. THE Documentation_System SHALL включать описание всех ролей пользователей
4. THE Documentation_System SHALL включать описание всех сообщений об ошибках
5. THE Documentation_System SHALL включать описание всех индикаторов состояния
6. THE Documentation_System SHALL включать описание всех технических характеристик
7. THE Documentation_System SHALL включать описание всех результатов тестирования

### Requirement 15: Интеграция с существующей документацией

**User Story:** Как разработчик документации, я хочу использовать существующие материалы, чтобы не дублировать работу.

#### Acceptance Criteria

1. WHEN создаётся User_Manual, THE Documentation_System SHALL использовать информацию из файла Docs/Руководство_пользователя.txt
2. WHEN создаётся Technical_Specification, THE Documentation_System SHALL использовать информацию из файла Docs/Спецификация.txt
3. WHEN описываются тест-кейсы, THE Documentation_System SHALL использовать информацию из файла Docs/Тест_кейсы.txt
4. WHEN описываются unit тесты, THE Documentation_System SHALL использовать информацию из файла Matieu.Tests/ServiceUpdateTimeTests.cs
5. WHEN описывается структура базы данных, THE Documentation_System SHALL использовать информацию из файла Docs/Табель_данных.txt
6. THE Documentation_System SHALL расширять существующую документацию новыми деталями
7. THE Documentation_System SHALL сохранять совместимость с существующими документами
