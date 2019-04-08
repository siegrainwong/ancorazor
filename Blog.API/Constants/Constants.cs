namespace Blog.API.Common
{
    public static class Constants
    {
        /// <summary>
        /// 用于 JWT 验证的 RSA 公密钥
        /// </summary>
        public static class RSAForToken
        {
            // openssl genrsa -out your_private_key_name.pem 2048
            public const string PrivateKey = @"MIIEpAIBAAKCAQEAq4M9Rkr3lETlRlJayOqBNEQd9wGhOoyAMJbpBPS9xOyzKmI7
                p0flk+P7trY0xCzkQAKjiIl9As6CXshunnL7LAwuSniGylBpKZWirsRX7OajxaSA
                /x7CtB6eMy/0jWo3DY3zUK5B7XtcJuPPcgLcSK4iYgrdm9ka+Z/Aqc32oCgviT1R
                iFp8UkDXW7opdPD/dnjnPCqjEJZ+yiDsxp6gN5+tZj3O2JAXOB6LAV8nIPl2TKQZ
                9i4fQra9M6KionLN7DK1jXU/3mX5hkegLRhdcYzUVAxSltx7QHn00DVZmBPge3Oj
                XOKOui1TBQvwGQwvFCe/x41lyKiAONvdtsmf8QIDAQABAoIBAQCVH7jpI0l8WyLW
                L8jLpEnOveMn3vzmQT88ABp+uqU9UWi+U0b2vWZ5XeKADJB4eWfD2AeEMbbF9/QW
                oUK89dA5jiW5jm3z/lJRW89lEUY5rpd6wvt91qsHnQ+EBhAl3NIdMOz7m2erKnUH
                Q1AdGyuY7f7rK/NfDDeODz6e5Xun55xsRhSnGsAqpkmR/30asjGK3nxdteC+kzRl
                qlrAUn46TmWyVE0k6x/QSbjfASBbANrJQMscaLgN9UNQ83qjFgaUR+Zpzl+l0WeH
                +vtk+/jKT/iG6Nppgr6r/s9Zpb/7QD0XJmAoACXCnQ/vRiCI85HUcesAeZwPWSLo
                QVABCEOhAoGBANViyWnAb3DvPiri6Jm+jHvL4cb+oW5NiGsbTTWjXCoGzUcPqazT
                ueN2L+vY12aCdUZEk61b1SyJfC17oxLlBIH6cVJ+ioxOQkKhNg17g3WDHCa88dTL
                zFaOCNrRJoTNbjxLJL2A6fDI3eAiiEXZbLQlRsmjJrtbB+jf0xkh3TzNAoGBAM3D
                tgq/Ef++mHLWyDPmYJPjl8kJhyoV2s1sSZwLRbPxazi8DLDhLEKRZUkH8M1nmg8L
                xueo/DoQUTm/GiICTyMdIl7/jGTsQtDB2lx3s4UXhsc0D7ZKhfLeyVuopoEwG6TI
                07QiCYBcD5jIhQOA6ys3LPGRIRuxSklLl10EBC+1AoGAAeRHTtWy5zhnv6+VSk+j
                JTHQhZgaTEUJsZFjZNdW6NR4m2mrImoaGscgc6HPfwwnCAFxobbPs/5gCHMxJei7
                2n8i5A1VIxtKgRa2yPXQW4lXBYzlQ/KulBHcSDxUcBb2JDiyUa/D4yuUs/j6Zkwg
                J5SxBPaaUEzlTA436+Ad+v0CgYBNmzk3yGTzfmFlPtj3qjZW0QpYir1uBBwoSmVg
                82dwvOdh2Js4w1S8LrZy2wnZju+uKRT2HugyxiC8lPU4SoKqjbx+9AdxsSJqNhMz
                uPn+gqcUzu/2IxluRtTPO0bBhvGGLzCZyhSnUxGW4Fo6vg70l7Tdz40bMrz/9AC8
                1lz12QKBgQCERRmGdgc5KJjTl2Vv16Jv/t7DNf9R3N+R6wPiZs/hFbti4x5piPkD
                w3Q7ewFqvZRqp7M4eyx3ixbP8NWDyfgEhfLYGt2TCu0l1MgR7+L/k7bpiARd8EDN
                vVG8kwK4lMCyPujJSfK7DzW3hW+PFwRNObJS/mY/Ksjp1lom5JCROQ==";

            // openssl rsa -pubout -in your_private_key_name.pem -out your_public_key_name.pem
            public const string PublicKey = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAq4M9Rkr3lETlRlJayOqB
                NEQd9wGhOoyAMJbpBPS9xOyzKmI7p0flk+P7trY0xCzkQAKjiIl9As6CXshunnL7
                LAwuSniGylBpKZWirsRX7OajxaSA/x7CtB6eMy/0jWo3DY3zUK5B7XtcJuPPcgLc
                SK4iYgrdm9ka+Z/Aqc32oCgviT1RiFp8UkDXW7opdPD/dnjnPCqjEJZ+yiDsxp6g
                N5+tZj3O2JAXOB6LAV8nIPl2TKQZ9i4fQra9M6KionLN7DK1jXU/3mX5hkegLRhd
                cYzUVAxSltx7QHn00DVZmBPge3OjXOKOui1TBQvwGQwvFCe/x41lyKiAONvdtsmf
                8QIDAQAB";
        }
    }
}
