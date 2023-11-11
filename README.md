The examples in .NET 8 for the [DDD and EF Core](https://www.pluralsight.com/courses/ddd-ef-core-preserving-encapsulation) course by Vladimir Khorikov

This repo contains several SQL you should apply to create/update the database during the course. They are numbered from 1 to 4 and introduced in commits where were needed first. The app uses an SQL  Server provider, and you can adjust connection settings in "appsettings.json".

I'm not a fan of Vladimir's approach to model `ValueObject`s with fully-fledged Entity fields inside them. I'd used a .NET 8 `ComplexProperty` as the Student's Name property until the final module. I reverted it to Owned Type only to adhere to the course's domain model.
