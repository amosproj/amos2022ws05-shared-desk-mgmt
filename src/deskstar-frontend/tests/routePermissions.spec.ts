import { test, expect } from "@playwright/test";

test("Admins can access home route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/");
  await expect(page).toHaveURL("/");
});

test("Admins can access bookings route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/bookings");
  await expect(page).toHaveURL("/bookings");
});

test("Admins can access book a desk route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/bookings/add");
  await expect(page).toHaveURL("/bookings/add");
});

test("Admins can access users overview route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/users");
  await expect(page).toHaveURL("/users");
});

test("Admins can access users requests route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/users/requests");
  await expect(page).toHaveURL("/users/requests");
});

test("Admins can access archieved users route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/users/restoration");
  await expect(page).toHaveURL("/users/restoration");
});

test("Admins can access resource overview route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/resources");
  await expect(page).toHaveURL("/resources");
});

test("Admins can access resource restoration route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/resources/restoration");
  await expect(page).toHaveURL("/resources/restoration");
});

test("Admins accessing unknown route defaults to 404 message", async ({
  page,
}) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("alice.admin@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/notexisting");
  await expect(
    page.getByRole("heading", { name: "This page could not be found." })
  ).toHaveCount(1);
});

// '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
test("Employees can access home route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/");
  await expect(page).toHaveURL("/");
});

test("Employees can access bookings route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/bookings");
  await expect(page).toHaveURL("/bookings");
});

test("Employees can access book a desk route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/bookings/add");
  await expect(page).toHaveURL("/bookings/add");
});

test("Employees can't access users overview route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/users");
  await page.waitForLoadState("networkidle");
  await expect(page).toHaveURL("/500");
  // await expect(
  //   page.getByRole("heading", { name: "This page could not be found." })
  // ).toHaveCount(1);
});

test("Employees can't access users requests route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/users/requests");
  await expect(page).toHaveURL("/500");
  // await expect(
  //   page.getByRole("heading", { name: "This page could not be found." })
  // ).toHaveCount(1);
});

test("Employees can't access archieved users route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/users/restoration");
  await expect(page).toHaveURL("/500");
  // await expect(
  //   page.getByRole("heading", { name: "This page could not be found." })
  // ).toHaveCount(1);
});

test("Employees can access resource overview route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/resources");
  await expect(page).toHaveURL("/500");
  // await expect(
  //   page.getByRole("heading", { name: "This page could not be found." })
  // ).toHaveCount(1);
});

test("Employees can access resource restoration route", async ({ page }) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/resources/restoration");
  await expect(page).toHaveURL("/500");
  // await expect(
  //   page.getByRole("heading", { name: "This page could not be found." })
  // ).toHaveCount(1);
});

test("Employees accessing unknown route defaults to 404 message", async ({
  page,
}) => {
  await page.goto("/login");

  await page.getByPlaceholder("Email").click();
  await page.getByPlaceholder("Email").fill("bob.employee@acme.com");
  await page.getByPlaceholder("Email").press("Tab");
  await page.getByPlaceholder("Password").fill("test123");
  await page.getByRole("button", { name: "Login" }).click();

  await page.waitForLoadState("networkidle");
  await page.goto("/notexisting");
  await expect(
    page.getByRole("heading", { name: "This page could not be found." })
  ).toHaveCount(1);
});
