export const environment = {
  production: false,
  apiUrl: 'https://localhost:7233'
};
  
export const deployed_environment = {
  production: true,
  apiUrl: 'https://proiect-net-2024-production.up.railway.app'
};

export const currentEnvironment = (() => {
  if (typeof window !== 'undefined' && window.location.hostname.includes('vercel.app')) {
      return deployed_environment;
  }
  return environment;
})();
  