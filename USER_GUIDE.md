# Task Management API: Exploratory Testing User Guide

Welcome! This guide is designed for manual Exploratory testers to quickly navigate and test the core features of the Task Management API. Our goal is to ensure the API behaves as expected across all main operations.

We will be using the interactive **Swagger UI** to perform our testing.

---

## 🛠 Prerequisites

Before starting your exploratory testing, ensure you have the following ready:

1. **API Access:** The API must be running locally.
   - If not already running, open a terminal in the project root and execute: `./run_locally.sh` (or `dotnet run --project src/TaskManagement.API`).
2. **Swagger UI:** Open your web browser and navigate to the Swagger interface:
   - URL: `http://localhost:5273/swagger`
3. **Familiarity with JSON:** Basic understanding of reading and editing simple JSON payloads.

---

## 🧪 Core Feature Testing Instructions

Follow these steps to manually test the core CRUD (Create, Read, Update, Delete) features of the Tasks endpoint.

### 1. Creating a New Task (POST)

Let's start by creating a brand-new task in the system.

1. **Locate** the `POST /api/Tasks` endpoint block in the Swagger UI.
2. **Click** the block to expand it.
3. **Click** the **'Try it out'** button located on the top right of the expanded section.
4. **Edit** the Request body with valid JSON. Here is an example:
   ```json
   {
     "title": "Exploratory Testing Task",
     "description": "Testing the create endpoint manually.",
     "status": 0,
     "categoryId": 1
   }
   ```
   *(Note: Status `0` typically represents 'Pending' or similar default state. CategoryId `1` assumes default seeded data exists).*
5. **Click** the large blue **'Execute'** button below the request body.
6. **Verify** the response in the "Responses" section below:
   - Ensure the Server response code is `201` (Created).
   - Check the Response body for the returned object. **Note the `"id"` value**, you will need it for the next steps!

*[Screenshot: Swagger UI showing the successful 201 response body after creating a task, with the new Task ID clearly visible]*

> **Pro-Tip:** Try passing an invalid `categoryId` (like `9999`) or an empty string for the `title` to explore how the API handles validation errors (expecting a `400 Bad Request`).

---

### 2. Retrieving All Tasks (GET)

Next, verify that the task you just created is listed among all tasks.

1. **Locate** the `GET /api/Tasks` endpoint block.
2. **Click** the block to expand it.
3. **Click** **'Try it out'**.
4. **Click** **'Execute'**.
5. **Verify** the Server response code is `200` (Success).
6. **Scroll** through the Response body (JSON array) and locate the task you created in Step 1.

> **Pro-Tip:** The list should default to only showing *active* tasks. Keep this in mind for the deletion step later, where the API uses "soft deletes".

---

### 3. Retrieving a Specific Task (GET by ID)

Let's pull up the specific details of the task we created.

1. **Locate** the `GET /api/Tasks/{id}` endpoint block.
2. **Click** the block to expand it.
3. **Click** **'Try it out'**.
4. **Enter** the `id` you noted from Step 1 into the required **'id'** input field.
5. **Click** **'Execute'**.
6. **Verify** the Server response code is `200` (Success).
7. **Ensure** the Response body matches the details you submitted during creation.

*[Screenshot: The 'Parameters' section showing the entered ID and the resulting 200 Success response body below]*

> **Pro-Tip:** Enter an ID that you know does *not* exist (e.g., `999999`) to confirm the API correctly returns a `404 Not Found` error.

---

### 4. Updating an Existing Task (PUT)

Now, let's modify our task.

1. **Locate** the `PUT /api/Tasks/{id}` endpoint block.
2. **Click** to expand and then **Click** **'Try it out'**.
3. **Enter** the same `id` from Step 1 into the **'id'** input field.
4. **Edit** the Request body to change some details (e.g., update the status or title):
   ```json
   {
     "title": "Exploratory Testing Task - UPDATED",
     "description": "I have updated this description.",
     "status": 1,
     "categoryId": 1
   }
   ```
5. **Click** **'Execute'**.
6. **Verify** the Server response code is `204` (No Content), indicating successful update without returning data.
7. **Optional:** Repeat **Step 3 (GET by ID)** to verify your changes actually saved to the database!

> **Pro-Tip:** Try sending a PUT request where the ID in the URL path does not match an ID in the system to verify `404 Not Found` handling.

---

### 5. Deleting a Task (DELETE)

Finally, let's remove the task.

1. **Locate** the `DELETE /api/Tasks/{id}` endpoint block.
2. **Click** to expand and then **Click** **'Try it out'**.
3. **Enter** your task `id` into the **'id'** input field.
4. **Click** **'Execute'**.
5. **Verify** the Server response code is `204` (No Content).
6. **Verification:** Go back to **Step 2 (Retrieve All Tasks)** and **Execute** it again. Ensure your deleted task is *no longer listed* in the response.

*[Screenshot: The final confirmation showing the 204 No Content response after executing the DELETE operation]*

> **Pro-Tip:** This API uses "Soft Deletes" (setting an `IsArchived` flag). The record still exists in the database but is hidden from standard queries. If you try to GET the specific ID again (Step 3), it might return `404 Not Found` if the global filter applies correctly!

---

## 🚨 Troubleshooting Common Errors

Encountering issues during your first run? Check these common pitfalls:

* **Site cannot be reached (localhost refused to connect):**
  * *Cause:* The API is not running.
  * *Fix:* Return to your terminal and ensure you ran `./run_locally.sh` or `dotnet run` successfully without errors.
* **400 Bad Request on POST/PUT:**
  * *Cause:* Invalid JSON syntax (e.g., missing a comma, extra quotes) or failing data validation (like a non-existent `categoryId`).
  * *Fix:* Carefully check your request body against the provided examples. Ensure categories exist.
* **404 Not Found:**
  * *Cause:* You are requesting or trying to modify an ID that doesn't exist (or has been soft-deleted).
  * *Fix:* Double-check the `id` you are entering. When in doubt, run the `GET /api/Tasks` endpoint to see all currently available IDs.
* **Database Errors (SQLite):**
  * *Cause:* Migrations might not be applied if you ran `dotnet run` manually on a fresh pull without setting up the DB.
  * *Fix:* Stop the app and run the `./run_locally.sh` script, which automatically handles setting up the SQLite database and applying necessary migrations.
