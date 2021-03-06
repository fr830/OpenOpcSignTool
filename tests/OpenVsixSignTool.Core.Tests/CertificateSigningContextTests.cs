﻿using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xunit;

namespace OpenVsixSignTool.Core.Tests
{
    public class CertificateSigningContextTests
    {
        [Theory]
        [InlineData(@"certs\rsa-2048-sha256.pfx")]
        [InlineData(@"certs\rsa-2048-sha1.pfx")]
        public async Task ShouldSignABlobOfDataWithRsaSha256(string pfxPath)
        {
            var certificate = new X509Certificate2(pfxPath, "test");
            var config = new CertificateSignConfigurationSet
            {
                SigningCertificate = certificate,
                PkcsDigestAlgorithm = HashAlgorithmName.SHA256,
                FileDigestAlgorithm = HashAlgorithmName.SHA256
            };

            using (var context = new CertificateSigningContext(config))
            {
                using (var hash = SHA256.Create())
                {
                    var digest = hash.ComputeHash(new byte[] { 1, 2, 3 });
                    var signature = await context.SignDigestAsync(digest);
                    Assert.Equal(OpcKnownUris.SignatureAlgorithms.rsaSHA256, context.XmlDSigIdentifier);
                    Assert.Equal(SigningAlgorithm.RSA, context.SignatureAlgorithm);

                    var roundtrips = await context.VerifyDigestAsync(digest, signature);
                    Assert.True(roundtrips);
                }
            }
        }

        [Theory]
        [InlineData(@"certs\rsa-2048-sha256.pfx")]
        [InlineData(@"certs\rsa-2048-sha1.pfx")]
        public async Task ShouldSignABlobOfDataWithRsaSha1(string pfxPath)
        {
            var certificate = new X509Certificate2(pfxPath, "test");
            var config = new CertificateSignConfigurationSet
            {
                SigningCertificate = certificate,
                PkcsDigestAlgorithm = HashAlgorithmName.SHA1,
                FileDigestAlgorithm = HashAlgorithmName.SHA1
            };

            using (var context = new CertificateSigningContext(config))
            {
                using (var hash = SHA1.Create())
                {
                    var digest = hash.ComputeHash(new byte[] { 1, 2, 3 });
                    var signature = await context.SignDigestAsync(digest);
                    Assert.Equal(OpcKnownUris.SignatureAlgorithms.rsaSHA1, context.XmlDSigIdentifier);
                    Assert.Equal(SigningAlgorithm.RSA, context.SignatureAlgorithm);

                    var roundtrips = await context.VerifyDigestAsync(digest, signature);
                    Assert.True(roundtrips);
                }
            }
        }

        [Theory]
        [InlineData(@"certs\ecdsa-p256-sha256.pfx")]
        public async Task ShouldSignABlobOfDataWithEcdsaP256Sha256(string pfxPath)
        {
            var certificate = new X509Certificate2(pfxPath, "test");
            var config = new CertificateSignConfigurationSet
            {
                SigningCertificate = certificate,
                PkcsDigestAlgorithm = HashAlgorithmName.SHA256,
                FileDigestAlgorithm = HashAlgorithmName.SHA256
            };

            using (var context = new CertificateSigningContext(config))
            {
                using (var hash = SHA256.Create())
                {
                    var digest = hash.ComputeHash(new byte[] { 1, 2, 3 });
                    var signature = await context.SignDigestAsync(digest);
                    Assert.Equal(OpcKnownUris.SignatureAlgorithms.ecdsaSHA256, context.XmlDSigIdentifier);
                    Assert.Equal(SigningAlgorithm.ECDSA, context.SignatureAlgorithm);

                    var roundtrips = await context.VerifyDigestAsync(digest, signature);
                    Assert.True(roundtrips);
                }
            }
        }
    }
}
