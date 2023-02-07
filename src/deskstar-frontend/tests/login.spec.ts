import { test, expect } from "@playwright/test";

test("login with registered admin email and correct password", async ({
  page,
}) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  // Displays welcome page
  await expect(page).toHaveURL("/");
  await expect(
    page.getByRole("heading", {
      name: "Hello Alice Admin, welcome back to Deskstar",
    })
  ).toHaveCount(1);
});

test("login with registered employee email and correct password", async ({
  page,
}) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  // Displays welcome page
  await expect(page).toHaveURL("/");
  await expect(
    page.getByRole("heading", {
      name: "Hello Bob Employee, welcome back to Deskstar",
    })
  ).toHaveCount(1);
});

test("login with unknown email and password fails", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("trudy@unknown.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  // Displays welcome page
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/login");
  await expect(page.getByText("ErrorInvalidCredentials")).toHaveCount(1);
});

test("login with email without password fails", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("trudy@unknown.com");
  await page.getByRole("button", { name: "Login" }).click();

  // Displays welcome page
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/login");
  await expect(page.getByText("CredentialsSignin")).toHaveCount(1);
});
