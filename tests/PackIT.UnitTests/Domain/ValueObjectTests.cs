using PackIT.Domain.Exceptions;
using PackIT.Domain.Factories;
using PackIT.Domain.ValueObjects;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PackIT.UnitTests.Domain
{
    public class ValueObjectTests
    {
        [Fact]
        public void PackingListName_Create_Via_Constructor()
        {
            PackingListName packingListName = new PackingListName("joske");
            packingListName.Value.ShouldBe("joske");
        }
        [Fact]
        public void PackingListName_Create_From_String()
        {
            PackingListName packingListName = "joske";
            packingListName.Value.ShouldBe("joske");
        }

        [Fact]
        public void PackingListName_Create_String_From_PackingList()
        {
            PackingListName packingListName = new PackingListName("joske");
            string myValue = packingListName;
            myValue.ShouldBe("joske");
        }

        [Fact]
        public void PackingListName_Create_Via_Constructor_When_String_Is_Null()
        {
            string nullString = null;


            //ACT
            PackingListName packingListName;
            var exception = Record.Exception(() => packingListName = new PackingListName(nullString));

            //ASSERT
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<EmptyPackingListNameException>();
        }
        [Fact]
        public void PackingListName_Create_Via_Constructor_When_String_Is_WhiteSpace()
        {
            string whiteSpace = " ";


            //ACT
            PackingListName packingListName;
            var exception = Record.Exception(() => packingListName = new PackingListName(whiteSpace));

            //ASSERT
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<EmptyPackingListNameException>();
        }
    }
}
