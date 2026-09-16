# ConcatString

## Пример кода на языке Swiston:

>_Данная программа складывает строку с подстрокой и считает длину полученной строки_

```
main 
{
    var s1: string = "Hello, ";
    var name: string;
    var res: string;
    var len: int;
    print("Введите ваше имя: \n");
    read(name);
    res = s1 + name;

    len = length(res);

    print(res);
    print(len);

}
```