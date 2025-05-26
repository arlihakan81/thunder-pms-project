using NETCore.Encrypt.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thunder.Infrastructure.Identity
{
	public class PasswordHasher
	{

		public string Hash(string password)
		{
			var hashed = password.SHA1();
			return hashed;
		}

		public PasswordVerificationResult Verify(string password, string hashedPassword)
		{
			var hashed = Hash(password);
			if (hashed == hashedPassword)
			{
				return PasswordVerificationResult.Success;
			}
			return PasswordVerificationResult.Failed;
		}


	}

	public enum PasswordVerificationResult
	{
		Failed,
		Success
	}

}
