using Hl7.Fhir.Model;
using Swashbuckle.AspNetCore.Filters;

namespace NIHR.StudyManagement.Api.Documentation
{
    public class RegisterNewStudyBundleRequestExampleV1 : IExamplesProvider<Bundle>
    {
        public Bundle GetExamples()
        {
            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Batch
            };

            bundle.Entry = new List<Bundle.EntryComponent>();

            // Initialise and set short title
            var study = new ResearchStudy()
            {
                Id = "researchStudy-1",
                Label = new List<ResearchStudy.LabelComponent> {
                    new ResearchStudy.LabelComponent
                    {
                         Value = "Investigating the Role of Genetics in Autoimmune Diseases",
                         //Type = new CodeableConcept()
                         //{
                         //    Coding = new List<Coding> { new Coding() {
                         //        Display = "Investigating the Role of Genetics in Autoimmune Diseases"
                         //    } }
                         //}
                    }
                },
                Status = PublicationStatus.Active
            };
            study.Identifier = new List<Identifier>();

            // Protocol ID
            study.Identifier.Add(new Identifier()
            {
                Use = Identifier.IdentifierUse.Usual,
                Type = new CodeableConcept()
                {
                    //Coding = new List<Coding>
                    //{
                    //    new Coding()
                    //    {
                    //        Display = "Protocol ID"
                    //    }
                    //},
                    Text = "Protocol ID"
                },
                Value = "STU2024OLS"
            });

            // Edge ID
            study.Identifier.Add(new Identifier()
            {
                Use = Identifier.IdentifierUse.Usual,
                Type = new CodeableConcept()
                {
                    //Coding = new List<Coding>
                    //{
                    //    new Coding()
                    //    {
                    //        Display = "EDGE ID"
                    //    }
                    //},
                    Text = "EDGE ID"
                },
                Value = "PRJ2024OLS"
            });

            // Add practitioner
            var practitioner = new Practitioner()
            {
                Id = "practitioner-1",
                //Identifier = new List<Identifier> {
                //    new Identifier {
                //        Use = Identifier.IdentifierUse.Official,
                //        Value = "practitioner-1"
                //    }
                //},
                Name = new List<HumanName>
                {
                    new HumanName
                    {
                        Use = HumanName.NameUse.Usual,
                        Family = "Scott",
                        Given = new List<string> { "Oliver Fake" }
                    }
                },
                Telecom = new List<ContactPoint>
                {
                    new ContactPoint
                    {
                        Use = ContactPoint.ContactPointUse.Work,
                        System = ContactPoint.ContactPointSystem.Email,
                        Value = "oliver.fake.scott@nhs.uk"
                    }
                }
            };

            // Add organization
            var organization = new Organization
            {
                Id = "organization-1",
                //Identifier = new List<Identifier> {
                //    new Identifier{
                //        Use = Identifier.IdentifierUse.Official,
                //        Value = "Fake organization"
                //    }
                //},
                Active = true,
                Type = new List<CodeableConcept>(),
                Name = "Fake Organization name"
            };

            bundle.Entry.Add(new Bundle.EntryComponent() { Resource = practitioner });
            bundle.Entry.Add(new Bundle.EntryComponent() { Resource = organization });

            var practitionerRole = new PractitionerRole()
            {
                Id = "practitionerRole-1",
                //Identifier = new List<Identifier> { new Identifier { Value = "practitionerRole_1" } },
                Practitioner = new ResourceReference() { Reference = $"#{practitioner.Id}" },
                Organization = new ResourceReference() { Reference = $"#{organization.Id}" },
                Code = new List<CodeableConcept> {
                    new CodeableConcept{
                        Coding = new List<Coding>{

                            new Coding{
                                //System = "RTS url",
                                //Code = "CHF_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                                Display = "CHIEF INVESTIGATOR"
                            }
                        }
                    }
                }
            };

            bundle.Entry.Add(new Bundle.EntryComponent() { Resource = practitionerRole });

            study.AssociatedParty = new List<ResearchStudy.AssociatedPartyComponent> {
                new ResearchStudy.AssociatedPartyComponent{
                    Party = new ResourceReference() { Reference = $"{practitionerRole.Id}"},
                    Role = new CodeableConcept
                    {
                        Coding = new List<Coding>{

                            new Coding{
                                //System = "RTS url",
                                //Code = "CHF_INV@2.16.840.1.113883.2.1.3.8.5.2.3.5",
                                Display = "CHIEF INVESTIGATOR"
                            }
                        }
                    }
                }
            };

            bundle.Entry.Add(new Bundle.EntryComponent() { Resource = study });

            return bundle;
        }
    }
}
