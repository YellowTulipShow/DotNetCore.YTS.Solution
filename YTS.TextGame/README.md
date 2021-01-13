# 文本游戏

## 使用命令

```shell
dotnet run -p .\YTS.TextGame\ -- -p 'D:\Work\YTS.ZRQ\UndergraduateStudy\EnglishLevel4Words\Kingsoft' --extname '.md' --count-work-repeat 5 --re-input '([a-zA-Z]+) \| \[([^\[\]]+)\] \| ([^\r\n]+)' --re-print '[ $1 ] : $3' --re-answer '$1'
```
