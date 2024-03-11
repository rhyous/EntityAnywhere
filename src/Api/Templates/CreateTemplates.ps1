param ($entity,$idType)

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
cd $dir
cd ..

function createProj {

    param (
        $entity,
        $idType,
        $projType,
        $folder,
        $slnpath,
        $entityPrefix
    )
    
    cd .\${folder}\
    $combined = ${folder}+'.'+${entity}
    dotnet new ${projType} -I $idType -E ${entityPrefix}${entity} -o $combined
    cd ..
    dotnet sln add .\${folder}\$combined\$combined.csproj -s Custom\Plugins\${slnpath}
}

function createItem {

    param (
        $entity,
        $idType,
        $itemType,
        $folder,
        $entityPrefix,
        $entitySuffix
    )
    
    $combined = ${folder}+'.'+${entity}
    cd .\${folder}\$combined
    dotnet new ${itemType} -I $idType -E ${entityPrefix}${entity}${entitySuffix}
    cd ..\..
}


createProj $entity $idType entityint Interfaces EntityInterfaces I
createProj $entity $idType entity Entities Entities
createProj $entity $idType entityev Events Events
createProj $entity $idType entityserv Services Services
createProj $entity $idType entityweb WebServices WebServices

createItem ${entity} $idType entityii Interfaces I 2
createItem ${entity} $idType entityie Entities "" 2

cd $dir