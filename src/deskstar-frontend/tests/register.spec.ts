import { test, expect } from "@playwright/test";
import { exit } from "process";
import { getUsersInDb, deleteUserInDb } from "../lib/test_helpers";
const { Client } = require("pg");

let db;

test.beforeAll(async () => {
  db = new Client({
    host: process.env.DB__HOST,
    port: 5432,
    user: process.env.DB__USERNAME,
    password: process.env.DB__PASSWORD,
  });

  try {
    await db.connect();
    console.log("Connected to database");
  } catch (error) {
    console.error("Error connecting to database");
    exit(1);
  }
});

test.beforeEach(async ({ page }) => {
  // Ensure eve doesn't exist in db
  if (!db) throw new Error("db is undefined");
  await deleteUserInDb(db, "eve.employee@acme.com");
});

test("Registration works for correct input", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("eve");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").fill("employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("test123");
  await page.getByRole("button", { name: "Register" }).click();

  // Notifies successful registration and redirects to login
  await expect(page.getByText("Registration successful!")).toHaveCount(1);
  await expect(page).toHaveURL("/login");
});

test("Registration fails for existing email", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("bob");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").fill("Employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("test123");
  await page.getByRole("button", { name: "Register" }).click();

  // Displays error and stays on registration page
  await expect(page.getByText("Email adress already registered")).toHaveCount(
    1
  );
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/register");
});

test("Registration fails for not matching passwords", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("eve");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").fill("Employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("abc");
  await page.getByRole("button", { name: "Register" }).click();

  // Displays error and stays on registration page
  await expect(page.getByText("Passwords must be equal!")).toHaveCount(1);
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/register");
});

test("Registration fails with missing company name", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("eve");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").fill("Employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("test123");
  await page.getByRole("button", { name: "Register" }).click();

  // Displays error and stays on registration page
  await expect(page.getByText("Please select a company")).toHaveCount(1);
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/register");
});

test("Registration fails for missing email", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("eve");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").fill("Employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").press("Tab");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("test123");
  await page.getByRole("button", { name: "Register" }).click();

  // Displays error and stays on registration page
  //TODO: check error message
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/register");
});

test("Registration fails for missing firstname", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Lastname").click();
  await page.getByPlaceholder("Lastname").fill("Employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("test123");
  await page.getByRole("button", { name: "Register" }).click();

  // Displays error and stays on registration page
  //TODO: check error message
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/register");
});

test("Registration fails for missing lastname", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("eve");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("test123");
  await page.getByRole("button", { name: "Register" }).click();

  // Displays error and stays on registration page
  //TODO: check error message
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/register");
});

test("Registration fails for missing password", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("eve");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").fill("Employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.getByRole("button", { name: "Register" }).click();

  // Displays error and stays on registration page
  //TODO: check error message
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/register");
});

test("Company select suggests acme ltd", async ({ page }) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await expect(page.getByText("ACME Ltd.")).toHaveCount(1);
});

test.only("Registered users can't use the application before approval", async ({
  page,
}) => {
  await page.goto("/register");

  await page.getByPlaceholder("Company").click();
  await page.getByPlaceholder("Company").fill("A");
  await page.getByText("ACME Ltd.").click();
  await page.getByPlaceholder("Firstname").click();
  await page.getByPlaceholder("Firstname").fill("eve");
  await page.getByPlaceholder("Firstname").press("Tab");
  await page.getByPlaceholder("Lastname").fill("employee");
  await page.getByPlaceholder("Lastname").press("Tab");
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.locator('input[name="Password"]').fill("test123");
  await page.locator('input[name="Password"]').press("Tab");
  await page.getByPlaceholder("Repeat password").fill("test123");
  await page.getByRole("button", { name: "Register" }).click();

  await expect(page.getByText("Registration successful!")).toHaveCount(1);
  await expect(page).toHaveURL("/login");
  await page.waitForLoadState("networkidle");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("eve.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  // Notifies approval error and stays on login page
  await expect(page.getByText("ErrorUserNotApproved")).toHaveCount(1);
  await expect(page).toHaveURL("/login");
});
