## Supermarket checkout

Implement the code for a supermarket checkout that calculates the total price of a number of items. Goods are priced individually, however there are weekly special offers for when multiple items are bought. For example "Apples are 30 each or 2 for 45".

Weekly offers change frequently.

This week's rule:

| Item   |Price for 1 item | Offer                |
|--------|-----------------|----------------------|
| Apple  | 30              | 2 for 45             |
| Banana | 50              | 3 for 130            |
| Peach  | 60              |  -                   |

The checkout accepts the items in any order, so that if we scan an apple, a banana and another apple, we’ll recognise two apples and apply the discount of 2 for 45.
