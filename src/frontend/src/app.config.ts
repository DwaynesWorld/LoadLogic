interface AppConfiguration {
  auth: {
    domain: string;
    clientId: string;
  };
  endpoints: {
    orders: string;
    customers: string;
    locations: string;
  };
}

const local: AppConfiguration = {
  auth: {
    domain: "dev-loadlogic.us.auth0.com",
    clientId: "LeyfUG6j6aepEwpErM40uat8sFCGf3NB"
  },
  endpoints: {
    orders: "http://localhost:10000/o/v1",
    customers: "http://localhost:10000/c/v1",
    locations: "http://localhost:10000/l/v1"
  }
};

export enum Environment {
  Local,
  Dev,
  Staging,
  Production
}

const getCurrentEnvironment = () => {
  const hostname = window.location.hostname.toLowerCase();

  if (hostname.includes("localhost")) return Environment.Local;
  // TODO: Setup environments
  return Environment.Production;
};

export const getEnvironmentConfig = () => {
  const currentEnvironment = getCurrentEnvironment();

  switch (currentEnvironment) {
    case Environment.Local:
      return local;
    default:
      return local;
  }
};

export default {
  ...getEnvironmentConfig()
};
