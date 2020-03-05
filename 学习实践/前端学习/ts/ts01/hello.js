function buildName(firstName, lastName) {
    if (lastName === void 0) { lastName = 'Tom'; }
    return firstName + ' ' + lastName;
}
var tomcat = buildName('Tom', 'Cat');
var cat = buildName('Cat', undefined);
console.log(tomcat);
console.log(cat);
