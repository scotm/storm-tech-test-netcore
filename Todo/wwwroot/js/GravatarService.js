document.addEventListener("DOMContentLoaded", () => {
  const userDivs = Array.from(document.getElementsByClassName("userIdentity"));

  // Keep track of the users and the divs that need to be updated
  const emails = new Map();
  userDivs.forEach((userdiv) => {
    const email = userdiv.getAttribute("data-email");
    if (email) {
      if (!emails.has(email)) {
        emails.set(email, []);
      }
      emails.get(email).push(userdiv);
    }
  });

  // Get the gravatar info for each email and apply it to the user divs
  emails.forEach((results, email) => {
    fetch("/Gravatar?email=" + email)
      .then((response) => response.json())
      .then((data) => {
        const real_name = data.entry[0].name.formatted;
        results.forEach((result) => {
          result.innerHTML = real_name;
        });
      })
      .catch((error) => {
        console.log("Error:", error);
      });
  });
});
