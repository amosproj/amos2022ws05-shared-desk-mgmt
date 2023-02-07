export async function getUsersInDb(db) {
  try {
    const result = await db.query('SELECT * FROM public."User"');
    console.log(result);
    return result.rows;
  } catch (error) {
    console.error("Failed to fetch users", error);
    return;
  }
}

export async function deleteUserInDb(db, email: string) {
  try {
    await db.query(
      `DELETE FROM public."User" WHERE "MailAddress" IN ('${email}');`
    );
  } catch (error) {
    console.error("Failed to delete user", error);
    return;
  }
}
