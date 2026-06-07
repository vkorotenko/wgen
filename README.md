# wgen

## Для чего  эта программа?
Маркетплейс https://www.wildberries.ru/ не предоставляет прямого пути получить картинки из артикла nmId.

Для разовых вызовов это не критично, однако когда вызываешь список товаров например через
https://feedbacks-api.wildberries.ru/api/v1/feedbacks
получать 100 картинок через API это 33 секунды ожидания или 429 статус ответа.
Собственно поэтому и была написана эта программа. 

## Вызовы 
wgen simple program to generate image path from nmId for https://www.wildberries.ru/

**wgen g** - generate a javascript for create a full url to image based on **nmId**. 
Attention check **nmId** before generate 
by command wgen s **you_nmId**

**wgen s** **nmId** - check exist or no image. If not exist run  **wgen m** to create new **parsed.json** file. 
This command send request to server **https://www.wildberries.ru/** and create new parsed.json file contain ranges of basket 

**wgen m **nmId** - scan https://www.wildberries.ru/** for new basket and write to **parsed.json** file

**WARNING**: Don't delete **parsed.json** file! This file contain all parsed data. 
This cached values improve generation speed.



## Retcodes: 
 * 0 - normal
 * 1 - single check **nmId** not passed, need recalculate base, run **wgen m**
 * 2 - выход за пределы диапазона. Более 90 ошибок подряд.


 ## Для  пользователей контейнеров и CI/CD
 Программа скомпилирована без зависимостей. Поддерживаемые платформы Linux и  Windows
 На macOS не проверял, хотя должно работать. 
 Для получения артефактов используйте следующую команду
        
    
    wmgen m nmId
    wmgen g > wb.js или  копируете wbnmid.js
    
  Это перенаправит вывод программы в файл. 

  ## Помощь в разработке

  Вы сможете поддержать разработку сделав перевод https://www.tinkoff.ru/rm/r_zaJRKMfLcN.efjSYxgZnZ/kzcpK43850
  Сервис предоставлен Т-Банком


  ## Другие  ссылки

  https://dev.wildberries.ru/forum/topics/1787/poluchenie-mediafaila-po-nmid-ili-inache

  Приятный метод, но нужны разрешения к API и куча запросов и кэширование

  https://dev.wildberries.ru/en/forum/topics/2225/poluchenie-kartinki-tovara-po-nmid
  Мой запрос. Собственно в нем посоветовали действовать через API

  ## Компиляция на других платформах кроме Windows

  Производится штатно, на поддерживаемых платформах. 
  Если вы захотите выложить релиз, то кидайте пул реквест с артефактами. 


 