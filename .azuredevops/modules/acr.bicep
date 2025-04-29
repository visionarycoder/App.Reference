param name string
param location string
resource acr 'Microsoft.ContainerRegistry/registries@2023-01-01-preview' = {
  name: name
  location: location
  sku: { name: 'Basic' }
  properties: { adminUserEnabled: true }
}
output loginServer string  = acr.properties.loginServer
output username     string = listCredentials(acr.id, '2023-01-01-preview').username
output password     string = listCredentials(acr.id, '2023-01-01-preview').passwords[0].value
