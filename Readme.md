# FilterExpressionParser

**FilterExpressionParser** is a `.NET` library that provides a powerful and intuitive way to filter API query results using query parameters. It allows developers to easily implement complex filtering expressions like:


## Features

- **Flexible Filtering**: Supports a wide range of filter operators such as `equals`, `greater than`, `less than`, and `partial match`.
- **Customizable**: Easily extendable to support custom operators or filtering logic.
- **Type-Safe**: Utilizes `.NET` type safety to ensure accurate filtering.
- **Lightweight**: Minimal performance overhead with robust parsing.

## Supported Operators

| Operator | Description              | Example Query     |
|----------|--------------------------|-------------------|
| `eq`     | Equal to                 | `field=eq:value`  |
| `ne`     | Not equal to             | `field=ne:value`  |
| `gte`    | Greater than or equal to | `field=gte:value` |
| `lte`    | Less than or equal to    | `field=lte:value` |
| `pm`     | Partial match            | `field=pm:value`  |
| `lt`     | Less than                | `field=lt:value`  |
| `gt`     | Greater Than             | `field=gt:value`  |

## Example Query


```http
GET http://localhost:5023/api/movies?yearOfRelease=gte:2002&yearOfRelease=lte:2024&title=pm:Nick

