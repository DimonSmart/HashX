# XorHash

[![License](https://img.shields.io/github/license/DimonSmart/HashX)](https://github.com/DimonSmart/HashX/LICENSE)

## Introduction

Welcome to the documentation for XorHash! This library offers a variable length hash function implementation with two unique features:

1. **Customizable Hash Size**: With XorHash, you have the freedom to tailor the hash size to your exact requirements. Whether you need a compact 32-bit hash or want to explore even smaller 1-byte hash sizes for educational purposes to demonstrate high collision edge cases, this function empowers you to select the hash size that aligns perfectly with your application's needs.

2. **Rolling Hash**: In addition to standard hashing, this function supports a rolling hash implementation. Not only can you add new bytes to the hash, but you can also subtract them, making it a versatile choice for various data processing tasks.

## Disclaimer

Please note that this hash function is **not suitable for cryptographic purposes** due to its lack of an avalanche effect. The avalanche effect is a crucial property of cryptographic hash functions where even a minor change in input data results in a significantly different output, making it extremely difficult for an attacker to predict or reverse-engineer the input data. This function does not exhibit this property and should not be used for cryptographic purposes.

## How to Use

To use this hash function, follow these steps:

1. Install the NuGet package: TODO
2. Import the library into your project.
3. Create an instance of the hash function with your desired hash size and rolling hash configuration.
4. Use the hash function to compute hashes or perform rolling hash operations.

csharp
// TODO: Add code examples here

## Details
**Collision**: A collision occurs when two distinct inputs produce the same hash output. In general, it is impossible to completely avoid collisions in hash functions due to the finite range of possible hash values compared to the infinite number of potential inputs. However, there are specially designed hash functions, particularly suited for well-known inputs, that are engineered to be collision-free.

**Freedom to Select Hash Size**: XorHash offers the unique advantage of allowing users to precisely customize the hash size in bytes, even at runtime. This flexibility empowers developers to adapt the hash function according to the specific requirements of their applications, ensuring that the hash size in bytes aligns perfectly with the data they are working with.

**Top 5 Hash Functions and Their Sizes**

| Hash Function        | Description                              | Hash Size (Bits) | Hash Size (Bytes) |
|----------------------|------------------------------------------|------------------|-------------------|
| SHA-256              | Secure Hash Algorithm                    | 256              | 32                |
| MD5                  | Message Digest Algorithm                 | 128              | 16                |
| SHA-1                | Secure Hash Algorithm                    | 160              | 20                |
| CRC32                | Cyclic Redundancy Check                  | 32               | 4                 |

## Corellation between Hash Size and Input Buffer ##

**One Byte Input**: In the edge case where the input is just one byte, a one-byte hash is sufficient and 100% collision-free, as there is only one possible input and, consequently, only one possible hash value.

**Two Bytes Input**: With a two-byte input, there are 65536 (2^16) possible unique inputs. If you use a one-byte hash, it would lead to a 99.9985% collision rate because there are only 256 (2^8) possible hash values. To achieve a collision-free hashing in this scenario, you should use a two-byte hash, which provides 65,536 (2^16) unique hash values, matching the number of possible inputs.

###Selecting Appropriate Hash Size Depending on Buffer Size###

**Selecting Appropriate Hash Size Depending on Buffer Size**: When designing an application that uses hash functions, it's essential to consider realistic parameters such as the file size and buffer size. In practice, very often, we can't examine all possible input buffer values; only a subset of them are relevant for a given application.

For example, consider the scenario where a hashing algorithm is applied to a sliding window of 256 bytes over a 1024 KB file to generate hashes. In this case, we can calculate the actual number of relevant input buffer values, which is 1,048,321. Using this information, we can analyze the probability of hash collisions and choose an appropriate hash size accordingly.

For practical purposes, different hash sizes can be examined to determine the likelihood of a collision. For example, when working with a 256-byte buffer, the following hash sizes yield these probabilities:

| Hash Size (Bytes) | Collision Probability                |
|-------------------|---------------------------------------|
| 1                 | 1.000000000000000                     |
| 2                 | 0.393469340287367                     |
| 4                 | 0.000000000012791                     |
| 8                 | 0.000000000000000000000000000000000   |

As seen in the table above, selecting a large enough hash size greatly reduces the collision probability.
When working with hash functions in real-world applications, it's crucial to understand the relationship between hash size and buffer size for minimizing hash collisions. By considering relevant parameters like file size and the sliding window size, the appropriate hash size can be chosen to balance the trade-offs between computational efficiency and data storage required.
