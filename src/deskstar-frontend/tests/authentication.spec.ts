import { test, expect } from "@playwright/test";

test("Show login, when not logged in", async ({ page, browser }) => {
  await page.goto("/");

  await page.waitForLoadState("networkidle");

  await expect(page).toHaveURL(/\/login.*/);

  await page.goto("/test");

  await page.waitForLoadState("networkidle");

  await expect(page).toHaveURL(/\/login.*/);
});

test("Test login", async ({ page }) => {
  await page.goto("/login");
});
