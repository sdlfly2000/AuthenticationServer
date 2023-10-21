const PROXY_CONFIG = [
  {
    context: [
      "/Authentication/Authenticate",
      "/UserManager/Users"
    ],
    target: "https://localhost:7008",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
