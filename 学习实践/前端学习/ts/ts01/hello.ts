function buildName(firstName: string, lastName: string = 'Tom') {
    return firstName + ' ' + lastName;
}
let tomcat = buildName('Tom', 'Cat');
let cat = buildName('Cat', undefined);

console.log(tomcat);

console.log(cat);