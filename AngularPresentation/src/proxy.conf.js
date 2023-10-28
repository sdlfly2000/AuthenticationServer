const PROXY_CONFIG = [
  {
    context: [
      "/api/**"
    ],
    target: "http://192.168.71.151:4202",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
