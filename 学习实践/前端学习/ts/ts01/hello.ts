class Animal {
    public name: string;
    constructor(name: string) {
        this.name = name;
    }
    SayHi() {
        return `Hi ${this.name}`
    }
}

class Cat extends Animal {
    SayHi() {
        return `${super.SayHi()} #Cat#`
    }
}
class Dog extends Animal {
    SayHi() {
        return `${super.SayHi()} #Dog#`
    }
}

let a: Animal = new Animal("薇薇");
console.log(a.SayHi())
let cat = new Cat("薇薇");
console.log(cat.SayHi())
let dog = new Dog("薇薇");
console.log(dog.SayHi())
