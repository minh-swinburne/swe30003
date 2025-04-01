# Question

in the domain entities, there's an entity called Identity, which is supposed to store a user's legal information.

1. Should it stay separate from the User entity (linked via 1-1 relationship) like currently, or should it be merged with User? My idea is that when register a new user, they don't have to provide Identity yet, but it's required later to book a ride, etc. (should booking a ride requires identity confirmed, btw?)
2. Should the Identity have `Sex` or `Gender`? it's the legal information, so maybe `Sex` would be more accurate?
3. What else should be included in the Identity? Currently I have NationalId, Nationality, LegalName, Gender, Dob, Address_1, Address_2 (or should 1 address only?)

---

# Answer

1. **Should Identity Stay Separate or Be Merged with User?**

   ✅ **Keep it separate.** Since users can register without providing their legal identity, it makes sense for `Identity` to be a separate entity. This way, the system can allow users to sign up and explore the platform but **require identity verification when performing critical actions** (e.g., booking a ride, registering as a driver).

   * 🔹  **Booking a Ride** : Yes, requiring identity confirmation before booking makes sense to **prevent fraudulent activity and ensure safety** (e.g., a verified passenger profile for drivers to trust).
   * 🔹  **Registering as a Driver** : Definitely requires identity confirmation.

   **Design Justification:**

   * Keeping `Identity` separate allows flexible enforcement of verification  **only when needed** .
   * It follows  **separation of concerns** , meaning `User` handles login/authentication, while `Identity` deals with legal verification.
   * A **one-to-one relationship (`User` → `Identity`)** ensures that a user  **can have only one legal identity** .

   **Alternative Approach?**

   * If you merged it into `User`, you’d have to allow `NULL` values for identity-related fields, making the schema less clean.
   * If you anticipate allowing  **multiple identities per user (unlikely in most cases)** , you’d need a **one-to-many (`User` → `Identities`)** relationship instead.

---

2. **Sex vs. Gender in Identity?**

   ✅ **Use `Sex`, not `Gender`, in the `Identity` entity.**

   * `Sex` refers to **biological/legal classification** (Male/Female) and is used in  **official documents (passports, ID cards)** .
   * `Gender` is **socially constructed** and might not be reflected in legal identification.

   **Recommendation:**

   * Use `Sex` in `Identity`.
   * If gender is needed for personalization (e.g., preferred pronouns), store it **separately in `User`** as an optional field.

---

3. **What Else Should Be Included in `Identity`?**

   ✅ Current fields:

   * `NationalId` (or equivalent ID document number)
   * `Nationality`
   * `LegalName`
   * `Sex`
   * `Dob`
   * `Address_1`
   * `Address_2` (or should 1 address only?)

   **Additional fields to consider:**

   * `IssuedCountry` → Some IDs are issued in a different country than the nationality.
   * `IdType` → Passport, Driver’s License, National ID, etc.
   * `IdExpiryDate` → Some IDs expire (e.g., passports, licenses).
   * `VerificationStatus` (Boolean or Enum) → Tracks whether the identity is verified.

   **Address Consideration:**

   * If `Address_1` and `Address_2` refer to different locations (e.g., permanent vs. current address), then keep both.
   * If they’re just  **different address lines for the same location** , rename them to `StreetAddress` and `City/State/PostalCode`.

   **Final `Identity` Entity Structure:**

   ```
   Identity
   ├── Id (PK)
   ├── UserId (FK → User)
   ├── LegalName
   ├── Sex
   ├── Dob
   ├── NationalId
   ├── IdType
   ├── IdExpiryDate (optional)
   ├── Nationality
   ├── IssuedCountry (optional)
   ├── VerificationStatus (Pending, Verified, Rejected)
   ├── Address_1
   ├── Address_2 (optional)
   ├── CreatedAt, UpdatedAt
   ```

---

### **Final Verdict:**

1. ✅ Keep `Identity` separate from `User` with a **one-to-one** relationship.
2. ✅ Use `Sex` in `Identity`, and optionally store `Gender` in `User`.
3. ✅ Include `IdType`, `IdExpiryDate`, and `VerificationStatus`.
4. ✅ Clarify address usage (permanent vs. temporary) before finalizing structure.

What do you think? 🚀
