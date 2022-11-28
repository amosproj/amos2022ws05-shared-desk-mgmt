import { NextApiRequest, NextApiResponse } from "next";
import {
  AuthResponse,
  register as authServiceRegister,
} from "../../lib/api/AuthService";

export default async function register(
  req: NextApiRequest,
  res: NextApiResponse
) {
  if (req.method !== "POST") {
    res.status(400).json({
      error: "Wrong method",
    });
    return;
  }

  console.log(req.body);
  const { companyId, firstName, lastName, mailAddress, password } = req.body;

  if (!(mailAddress && password && companyId && firstName && lastName)) {
    res.status(400).json({
      error: "user values required",
    });
    return;
  }

  const response = await authServiceRegister({
    companyId,
    firstName,
    lastName,
    mailAddress,
    password,
  });

  if (response !== AuthResponse.Success) {
    switch (response) {
      case AuthResponse.ErrorCompanyNotFound:
        return res.status(400).json({
          error: "Company not Found",
        });
      case AuthResponse.ErrorEmailaddressAlreadyExists:
        return res.status(400).json({
          error: "Email adress already registered",
        });
      default:
        return res.status(400).json({
          error: "unknown error",
        });
    }
  }

  res.json(response);
}
