param location string = resourceGroup().location
param prfx      string = 'praetor'
param containerTag string = 'latest'     // overridden by azd env set

// 1. Registry
module acr 'modules/acr.bicep' = {
  name: '${prfx}-acr'
  params: { name: '${prfx}acr' location }
}

// 2. App-Insights + LogAnalytics (implicit from aca-env)
module env 'modules/aca-env.bicep' = {
  name: '${prfx}-acaenv'
  params: {
    name:  '${prfx}-env'
    location
    logWorkspaceName: '${prfx}-la'
  }
}

// 3. Container App (silo + API in single image)
module api 'modules/container-app.bicep' = {
  name: '${prfx}-api'
  params: {
    appName: '${prfx}-api'
    envName: env.outputs.envName
    registryLoginServer: acr.outputs.loginServer
    registryUsername:     acr.outputs.username
    registryPassword:     acr.outputs.password
    image: '${acr.outputs.loginServer}/${prfx}-api:${containerTag}'
    location
  }
}

// 4. Azure SQL
module sql 'modules/sql.bicep' = {
  name: '${prfx}-sql'
  params: {
    serverName: '${prfx}-sql'
    dbName:     'Praetor'
    adminLogin: 'sqladmin'
    location
  }
}

// 5. Service Bus
module sb 'modules/servicebus.bicep' = {
  name: '${prfx}-sb'
  params: {
    nsName:  '${prfx}-sb'
    topic:   'Praetor.Events'
    location
  }
}

// Expo-secrets for azd
output ACR_SERVER   string = acr.outputs.loginServer
output SQL_CONNSTR  string = sql.outputs.fullConn
output SB_CONNSTR   string = sb.outputs.sbConnStr
output ACA_ENDPOINT string = api.outputs.url
